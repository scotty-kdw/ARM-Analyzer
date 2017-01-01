using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ARMAnalyzer
{
    public class Entry
    {
        public UInt32 Count = 0;
        public UInt32 BitMask = 0;
        public UInt32 Address = 0;
        public Byte[] Opcode = new Byte[4];
        public String Mnemonic = "";
        public String OperandStr = "";
        public UInt32[] Operands = new UInt32[16];
        public String[] Operands_Name = new String[16];
        public UInt32 Operand_Cnt = 0;

        public List<String>         Operands_Dst = new List<String>();
        public List<List<String>>   Operands_Src = new List<List<String>>();

        private List<string> cond_list = new List<string>(new string[] { "eq", "ne", "cs", "hs", "cc", "lo", "mi", "pl", "vs", "vc", "hi", "ls", "ge", "lt", "gt", "le", "al", "nv" });

        private void OperandsParsing()
        {
            string full = this.OperandStr.ToString().Trim();
            char[] separate = new char[] { ',', ' ', '[', '{', '#' };
            string[] operands = full.Split(separate, StringSplitOptions.None);

            foreach (string operand in operands)
            {
                if (!String.IsNullOrEmpty(operand))
                {
                    this.Operands_Name[Operand_Cnt++] = operand.TrimEnd(']', '}');
                }
            }
        }

        public Boolean isThumb()
        {
            if ((this.BitMask & 0x00000020) == 0x00000020)
                return true;
            else
                return false;
        }

        public int thumb_size()
        {
            int size = (int)Convert.ToUInt32(this.Opcode[1]);
            //System.Windows.Forms.MessageBox.Show(thumb_size.ToString());

            if (((size & 0xe0) == 0xe0) && ((size & 0x18) != 0))
            {
                return 4;
            }
            else
            {
                return 2;
            }
        }

        public void SetEntry(Byte[] buff)
        {
            this.Count = BitConverter.ToUInt32(buff, 0);
            this.BitMask = BitConverter.ToUInt32(buff, 4);
            this.Address = BitConverter.ToUInt32(buff, 8);
            Array.Copy(buff, 12, this.Opcode, 0, 4);

            Byte[] str_buff = new Byte[64];
            Array.Copy(buff, 16, str_buff, 0, 16);
            this.Mnemonic = Encoding.UTF8.GetString(str_buff.TakeWhile(b => !b.Equals(0)).ToArray());
            //this.Mnemonic = Encoding.UTF8.GetString(str_buff);
            //this.Mnemonic = this.Mnemonic.Substring(0, this.Mnemonic.IndexOf('\0'));
            Array.Copy(buff, 32, str_buff, 0, 64);
            this.OperandStr = Encoding.UTF8.GetString(str_buff.TakeWhile(b => !b.Equals(0)).ToArray());
            this.OperandsParsing();

            int i;
            for (i = 0; i < 16; i++)
            {
                this.Operands[i] = BitConverter.ToUInt32(buff, 96 + (i * 4));
            }

            this.SrcDstParsing();
        }

        public String GetLine()
        {
            String line = "";

            // Thumb
            if ( isThumb() == true ) {
                if (thumb_size() == 2)
                {
                    line = String.Format("[THUMB] {0,10:d}    {1,8:X}    {2,2:X2} {3,2:X2} {4,2:X2} {5,2:X2} {6,16}    {7}"
                        , this.Count, (this.Address & 0xFFFFFFFE), this.Opcode[0], this.Opcode[1], "  ", "  ", this.Mnemonic, this.OperandStr);
                }
                else
                {
                    line = String.Format("[THUMB] {0,10:d}    {1,8:X}    {2,2:X2} {3,2:X2} {4,2:X2} {5,2:X2} {6,16}    {7}"
                        , this.Count, (this.Address & 0xFFFFFFFE), this.Opcode[0], this.Opcode[1], this.Opcode[2], this.Opcode[3], this.Mnemonic, this.OperandStr);
                }
            }
            // ARM
            else {
                line = String.Format("[  ARM] {0,10:d}    {1,8:X}    {2,2:X2} {3,2:X2} {4,2:X2} {5,2:X2} {6,16}    {7}"
                , this.Count, (this.Address & 0xFFFFFFFE), this.Opcode[0], this.Opcode[1], this.Opcode[2], this.Opcode[3], this.Mnemonic, this.OperandStr);
            }

            /*
            for (int i = 0; i < 16; i++)
            {
                line += String.Format("    {0,8:X}", this.Operands[i]);
            }
            */

            return line;
        }

        private uint submask(uint x)
        {
            return (uint)((1L << ((int)(x) + 1)) - 1);
        }
        private uint bit(uint obj, int st)
        {
            return (((obj) >> (st)) & 1);
        }
        private uint bits(uint obj, int st, int fn)
        {
            return (((obj) >> (st)) & submask((uint)(fn - st)));
        }
        private uint lsl(uint num, uint shift)
        {
            return (num << (int)shift);
        }
        private uint lsr(uint num, uint shift)
        {
            return (num >> (int)shift);
        }
        private int asr(int num, uint shift)
        {
            return (num >> (int)shift);
        }
        private void SrcDstParsingCheck()
        {
            String full = null;
            String line = null;
            int j = 0;
            foreach (string dst in this.Operands_Dst)
            {
                line = String.Format("{0} :", dst);
                foreach (string src in this.Operands_Src[j])
                {
                    line = String.Format("{0} {1}", line, src);
                }
                full = String.Format("{0}\n{1}", full, line);
                j++;
            }
            MessageBox.Show(this.GetLine() + full);
        }
        private void SrcDstParsing()
        {
            /*
            uint opcode;
            uint coproc, cond, op, op1, op2, A, B, R, imm5;
            uint Rd, Rt, Rt2, Rm, Rn;

            opcode = (uint)BitConverter.ToInt32(this.Opcode, 0);
             */

            // 0000 0000  0000 0000  0000 0000  0000 1111 : 4 byte
            uint byte_sign = 0xF;

            // NEON, VFP Instructions
            if (this.Mnemonic[0].Equals('v'))
            {
                if (this.Operands_Name[this.Operand_Cnt-1].EndsWith("!"))
                {
                    this.Operands_Name[this.Operand_Cnt - 1] = this.Operands_Name[this.Operand_Cnt - 1].Substring(0,2);
                    //this.Operands_Name[this.Operand_Cnt - 1] = this.Operands_Name[this.Operand_Cnt - 1].TrimEnd('!');
                    //this.Operands_Name[this.Operand_Cnt - 1] = this.Operands_Name[this.Operand_Cnt - 1].TrimEnd(']');

                    // Dst
                    this.Operands_Dst.Add(this.Operands_Name[this.Operand_Cnt - 1]);
                    // Src
                    List<String> item = new List<String>();
                    item.Add(this.Operands_Name[this.Operand_Cnt - 1]);
                    this.Operands_Src.Add(item);
                }

                //MessageBox.Show(this.Mnemonic.ToString());
                if (this.Mnemonic.Contains("vld1.8") || this.Mnemonic.Contains("vld1.32"))
                {
                    // 0000 0000  0000 0000  0000 0000  1111 1111 : 8 byte
                    byte_sign = 0xFF;
                    int operand_jump = 1;
                    if (this.OperandStr[3] == '[')
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 0001 : 1 byte
                        byte_sign = 0x1;
                        operand_jump = 2;
                    }
                    List<String> item = null;
                    for (int i = 0; i < this.Operand_Cnt - 1; i = i + operand_jump)
                    {
                        uint byte_check = byte_sign;
                        for ( int k = 0 ;
                            byte_check != 0 ;
                            k++, byte_check = byte_check >> 1 ) {
                            // Dst
                            this.Operands_Dst.Add(
                                String.Format("{0}.{1}", this.Operands_Name[i], k)
                                );
                            // Src
                            item = new List<String>();
                            item.Add(String.Format("0x{0,8:X8}", this.Operands[0] + (i/operand_jump * 8) + k));
                            this.Operands_Src.Add(item);
                        }
                    }
                    //SrcDstParsingCheck();
                }
                else if (this.Mnemonic.Contains("vld4.8"))
                {
                    // 0000 0000  0000 0000  0000 0000  0000 0001 : 1 byte
                    byte_sign = 0x1;

                    List<String> item = null;
                    for (int i = 0; i < this.Operand_Cnt - 1; i = i + 2 )
                    {
                        uint byte_check = byte_sign;
                        for ( int k = 0 ;
                            byte_check != 0 ;
                            k++, byte_check = byte_check >> 1 ) {
                            // Dst
                            this.Operands_Dst.Add(
                                String.Format("{0}.{1}", this.Operands_Name[i], k)
                                );
                            // Src
                            item = new List<String>();
                            item.Add(String.Format("0x{0,8:X8}", this.Operands[0] + (i / 2) + k));
                        }
                        this.Operands_Src.Add(item);
                    }
                    //SrcDstParsingCheck();
                }
                else if (this.Mnemonic.Contains("vst1.8") || this.Mnemonic.Contains("vst1.32"))
                {
                    // 0000 0000  0000 0000  0000 0000  1111 1111 : 8 byte
                    byte_sign = 0xFF;
                    int operand_jump = 1;
                    if (this.OperandStr[3] == '[')
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 0001 : 1 byte
                        byte_sign = 0x1;
                        operand_jump = 2;
                    }
                    List<String> item = null;
                    for (int i = 0; i < this.Operand_Cnt - 1; i = i + operand_jump)
                    {
                        uint byte_check = byte_sign;
                        for ( int k = 0 ;
                            byte_check != 0 ;
                            k++, byte_check = byte_check >> 1 ) {
                            // Dst 1
                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[0] + (i/operand_jump * 8) + k));
                            // Src 1
                            item = new List<String>();
                            item.Add(
                                String.Format("{0}.{1}", this.Operands_Name[i], k)
                                );
                            this.Operands_Src.Add(item);
                        }
                    }
                    //SrcDstParsingCheck();
                }
                else if (this.Mnemonic.Contains("vst4.8"))
                {
                    // 0000 0000  0000 0000  0000 0000  0000 0001 : 1 byte
                    byte_sign = 0x1;

                    for (int i = 0; i < this.Operand_Cnt - 1; i = i + 2)
                    {
                        uint byte_check = byte_sign;
                        for ( int k = 0 ;
                            byte_check != 0 ;
                            k++, byte_check = byte_check >> 1 ) {
                            // Dst
                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[0] + (i / 2) + k));
                            // Src
                            List<String> item = new List<String>();
                            item.Add(
                                String.Format("{0}.{1}", this.Operands_Name[i], k)
                                );
                            this.Operands_Src.Add(item);
                        }
                    }
                    //SrcDstParsingCheck();
                }
            }
            // ARM Instructions
            else
            {
                if (this.Mnemonic.Contains("ldr"))
                {
                    if (this.Mnemonic.Contains("ldrb"))
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 0001 : 1 byte
                        byte_sign = 0x1;
                    }
                    else if (this.Mnemonic.Contains("ldrsb"))
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 0001 : 1 byte
                        byte_sign = 0x1;
                    }
                    else if (this.Mnemonic.Contains("ldrh"))
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 0011 : 2 byte
                        byte_sign = 0x3;
                    }
                    else if (this.Mnemonic.Contains("ldrsh"))
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 0011 : 2 byte
                        byte_sign = 0x3;
                    }
                    else
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 1111 : 4 byte
                        byte_sign = 0xF;
                    }

                    if (this.Mnemonic.Contains("ldrex"))
                    {
                        // LDREX{cond} Rt, [Rn {, #offset}]
                        // LDREXB{cond} Rt, [Rn]
                        // LDREXH{cond} Rt, [Rn]
                        // LDREXD{cond} Rt, Rt2, [Rn]

                        return;
                    }

                    if (this.Operand_Cnt == 2)
                    {
                        // LDR{type}{cond} Rt, [Rn]                     ; immediate offset
                        if (!Char.IsDigit(this.Operands_Name[1][0]))
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[0]);
                            // Src
                            List<String> item = new List<String>();
                            for ( int i = 0 ;
                                byte_sign != 0 ;
                                i++, byte_sign = byte_sign >> 1 ) {
                                item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + i));
                            }
                            item.Add(this.Operands_Name[1]);
                            this.Operands_Src.Add(item);
                        }
                        // LDR{type}{cond}{.W} Rt, label
                        else
                        {
                            return;
                        }
                    }
                    else if (this.Operand_Cnt == 3)
                    {
                        // 3rd operand is register
                        if (!Char.IsDigit(this.Operands_Name[2][0]) && this.Operands_Name[2][0] != '-')
                        {
                            // LDR{type}{cond} Rt, [Rn, +/-Rm]               ; register offset
                            if (this.OperandStr.EndsWith("]"))
                            {
                                // Dst
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.Operands[2] + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            // LDR{type}{cond} Rt, [Rn, +/-Rm]!              ; pre-indexed
                            else if (this.OperandStr.EndsWith("!"))
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.Operands[2] + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            // LDR{type}{cond} Rt, [Rn], +/-Rm               ; post-indexed
                            else
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                        }
                        // 3rd operand is #offset
                        else
                        {
                            // LDR{type}{cond} Rt, [Rn, #offset]             ; immediate offset
                            if (this.OperandStr.EndsWith("]"))
                            {
                                // Dst
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.Operands[2] + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);
                            }
                            // LDR{type}{cond} Rt, [Rn, #offset]!            ; pre-indexed
                            else if (this.OperandStr.EndsWith("!"))
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.Operands[2] + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);
                            }
                            // LDR{type}{cond} Rt, [Rn], #offset             ; post-indexed
                            else
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);
                            }
                        }
                    }
                    else if (this.Operand_Cnt == 5)
                    {
                        // LDR{type}{cond} Rt, [Rn, +/-Rm, shift]        ; register offset
                        // ex)   LDR.W    r3, [r3, r0, lsl #2]
                        if (this.OperandStr.EndsWith("]"))
                        {
                            if (this.Operands_Name[3].Equals("lsl"))
                            {
                                // Dst
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsl(this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else if (this.Operands_Name[3].Equals("lsr"))
                            {
                                // Dst
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsr(this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else if (this.Operands_Name[3].Equals("asr"))
                            {
                                // Dst
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.asr((int)this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else
                            {
                                MessageBox.Show(this.GetLine());
                            }
                        }
                        // LDR{type}{cond} Rt, [Rn, +/-Rm, shift]!       ; pre-indexed
                        else if (this.OperandStr.EndsWith("!"))
                        {
                            if (this.Operands_Name[3].Equals("lsl"))
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsl(this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else if (this.Operands_Name[3].Equals("lsr"))
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsr(this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else if (this.Operands_Name[3].Equals("asr"))
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.asr((int)this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else
                            {
                                MessageBox.Show(this.GetLine());
                            }
                        }
                        // LDR{type}{cond} Rt, [Rn], +/-Rm, shift        ; post-indexed
                        else
                        {
                            if (this.Operands_Name[3].Equals("lsl"))
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsl(this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else if (this.Operands_Name[3].Equals("lsr"))
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsr(this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else if (this.Operands_Name[3].Equals("asr"))
                            {
                                // Dst 1
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src 1
                                List<String> item = new List<String>();
                                for ( int i = 0 ;
                                    byte_sign != 0 ;
                                    i++, byte_sign = byte_sign >> 1 ) {
                                    item.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.asr((int)this.Operands[2], this.Operands[3]) + i));
                                }
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else
                            {
                                MessageBox.Show(this.GetLine());
                            }
                        }
                    }
                    else if (this.Mnemonic.Length > 3)
                    {
                        if (this.Mnemonic[3] == 'd')
                        {
                            if (this.Operand_Cnt == 3)
                            {
                                // LDRD{cond}      Rt, Rt2, [Rn]                 ; immediate offset, doubleword
                                if (!Char.IsDigit(this.Operands_Name[2][0]) && this.Operands_Name[2][0] != '-')
                                {
                                    // Dst 1
                                    this.Operands_Dst.Add(this.Operands_Name[0]);
                                    // Src 1
                                    List<String> item = new List<String>();
                                    for ( int i = 0 ;
                                        byte_sign != 0 ;
                                        i++, byte_sign = byte_sign >> 1 ) {
                                        item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + i));
                                    }
                                    item.Add(this.Operands_Name[2]);
                                    this.Operands_Src.Add(item);

                                    // Dst 2
                                    this.Operands_Dst.Add(this.Operands_Name[1]);
                                    // Src 2
                                    item = new List<String>();
                                    for ( int i = 0 ;
                                        byte_sign != 0 ;
                                        i++, byte_sign = byte_sign >> 1 ) {
                                        item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + i));
                                    }
                                    item.Add(this.Operands_Name[2]);
                                    this.Operands_Src.Add(item);
                                }

                                // LDRD{cond}      Rt, Rt2, label                ; Doubleword
                                else
                                {
                                    // Dst 1
                                    this.Operands_Dst.Add(this.Operands_Name[0]);
                                    // Src 1
                                    List<String> item = new List<String>();
                                    for ( int i = 0 ;
                                        byte_sign != 0 ;
                                        i++, byte_sign = byte_sign >> 1 ) {
                                        item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + i));
                                    }
                                    this.Operands_Src.Add(item);

                                    // Dst 2
                                    this.Operands_Dst.Add(this.Operands_Name[1]);
                                    // Src 2
                                    item = new List<String>();
                                    for ( int i = 0 ;
                                        byte_sign != 0 ;
                                        i++, byte_sign = byte_sign >> 1 ) {
                                        item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + i));
                                    }
                                    this.Operands_Src.Add(item);
                                }
                            }

                            else if (this.Operand_Cnt == 4)
                            {
                                if (!Char.IsDigit(this.Operands_Name[2][0]) && this.Operands_Name[2][0] != '-')
                                {
                                    // LDRD{cond}      Rt, Rt2, [Rn, +/-Rm]          ; register offset, doubleword
                                    if (this.OperandStr.EndsWith("]"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }

                                    // LDRD{cond}      Rt, Rt2, [Rn, +/-Rm]!         ; pre-indexed, doubleword
                                    else if (this.OperandStr.EndsWith("!"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }

                                    // LDRD{cond}      Rt, Rt2, [Rn], +/-Rm          ; post-indexed, doubleword
                                    else
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                }
                                else
                                {
                                    // LDRD{cond}      Rt, Rt2, [Rn, #offset]        ; immediate offset, doubleword
                                    if (this.OperandStr.EndsWith("]"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);
                                    }

                                    // LDRD{cond}      Rt, Rt2, [Rn, #offset]!       ; pre-indexed, doubleword
                                    else if (this.OperandStr.EndsWith("!"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);
                                    }

                                    // LDRD{cond}      Rt, Rt2, [Rn], #offset        ; post-indexed, doubleword
                                    else
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);
                                    }
                                }
                            }

                            else if (this.Operand_Cnt == 6)
                            {
                                // LDRD{cond}      Rt, Rt2, [Rn, +/-Rm, shift]   ; register offset, doubleword
                                if (this.OperandStr.EndsWith("]"))
                                {
                                    if (this.Operands_Name[4].Equals("lsl"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsl(this.Operands[3], this.Operands[4]) + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsl(this.Operands[3], this.Operands[4]) + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else if (this.Operands_Name[4].Equals("lsr"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsr(this.Operands[3], this.Operands[4]) + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsr(this.Operands[3], this.Operands[4]) + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else if (this.Operands_Name[4].Equals("asr"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.asr((int)this.Operands[3], this.Operands[4]) + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.asr((int)this.Operands[3], this.Operands[4]) + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this.GetLine());
                                    }
                                }

                                // LDRD{cond}      Rt, Rt2, [Rn, +/-Rm, shift]!  ; pre-indexed, doubleword
                                else if (this.OperandStr.EndsWith("!"))
                                {
                                    if (this.Operands_Name[4].Equals("lsl"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsl(this.Operands[3], this.Operands[4]) + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsl(this.Operands[3], this.Operands[4]) + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else if (this.Operands_Name[4].Equals("lsr"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsr(this.Operands[3], this.Operands[4]) + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsr(this.Operands[3], this.Operands[4]) + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else if (this.Operands_Name[4].Equals("asr"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.asr((int)this.Operands[3], this.Operands[4]) + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.asr((int)this.Operands[3], this.Operands[4]) + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this.GetLine());
                                    }
                                }

                                // LDRD{cond}      Rt, Rt2, [Rn], +/-Rm, shift   ; post-indexed, doubleword
                                else
                                {
                                    if (this.Operands_Name[4].Equals("lsl"))
                                    {
                                        // Dst 1
                                        this.Operands_Dst.Add(this.Operands_Name[0]);
                                        // Src 1
                                        List<String> item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 2
                                        this.Operands_Dst.Add(this.Operands_Name[1]);
                                        // Src 2
                                        item = new List<String>();
                                        for ( int i = 0 ;
                                            byte_sign != 0 ;
                                            i++, byte_sign = byte_sign >> 1 ) {
                                            item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + i));
                                        }
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this.GetLine());
                                    }
                                }
                            }

                            else
                            {
                                MessageBox.Show("00LDRD\n" + this.GetLine());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("01LDRD_STR\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("str"))
                {
                    if (this.Mnemonic.Contains("strb"))
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 0001 : 1 byte
                        byte_sign = 0x1;
                    }
                    else if (this.Mnemonic.Contains("strh"))
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 0011 : 2 byte
                        byte_sign = 0x3;
                    }
                    else
                    {
                        // 0000 0000  0000 0000  0000 0000  0000 1111 : 4 byte
                        byte_sign = 0xF;
                    }

                    if (this.Mnemonic.Contains("strex"))
                    {
                        // STREX{cond} Rd, Rt, [Rn {, #offset}]
                        // STREXB{cond} Rd, Rt, [Rn]
                        // STREXH{cond} Rd, Rt, [Rn]
                        // STREXD{cond} Rd, Rt, Rt2, [Rn]

                        return;
                    }

                    if (this.Operand_Cnt == 2)
                    {
                        // STR{type}{cond} Rt, [Rn]                      ; immediate offset

                        for ( int k = 0 ;
                            byte_sign != 0 ;
                            k++, byte_sign = byte_sign >> 1 ) {
                            // Dst
                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + k));
                            // Src
                            List<String> item = new List<String>();
                            item.Add(this.Operands_Name[0]);
                            this.Operands_Src.Add(item);
                        }
                    }
                    else if (this.Operand_Cnt == 3)
                    {
                        // 3rd operand is register
                        if (!Char.IsDigit(this.Operands_Name[2][0]) && this.Operands_Name[2][0] != '-')
                        {
                            // STR{type}{cond} Rt, [Rn, +/-Rm]               ; register offset
                            if (this.OperandStr.EndsWith("]"))
                            {
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.Operands[2] + k));
                                    // Src
                                    List<String> item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }
                            }
                            // STR{type}{cond} Rt, [Rn, +/-Rm]!              ; pre-indexed
                            else if (this.OperandStr.EndsWith("!"))
                            {
                                List<String> item;
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst 1
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.Operands[2] + k));
                                    // Src 1
                                    item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            // STR{type}{cond} Rt, [Rn], +/-Rm               ; post-indexed
                            else
                            {
                                List<String> item;
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst 1
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + k));
                                    // Src 1
                                    item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                        }
                        // 3rd operand is #offset
                        else
                        {
                            // STR{type}{cond} Rt, [Rn, #offset]             ; immediate offset
                            if (this.OperandStr.EndsWith("]"))
                            {
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.Operands[2] + k));
                                    // Src
                                    List<String> item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }
                            }
                            // STR{type}{cond} Rt, [Rn, #offset]!            ; pre-indexed
                            else if (this.OperandStr.EndsWith("!"))
                            {
                                List<String> item;
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst 1
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.Operands[2] + k));
                                    // Src 1
                                    item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);
                            }
                            // STR{type}{cond} Rt, [Rn], #offset             ; post-indexed
                            else
                            {
                                List<String> item;
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst 1
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + k));
                                    // Src 1
                                    item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                this.Operands_Src.Add(item);
                            }
                        }
                    }
                    else if (this.Operand_Cnt == 5)
                    {
                        // STR{type}{cond} Rt, [Rn, +/-Rm, shift]        ; register offset
                        // ex)   STR.W    r3, [r3, r0, lsl #2]
                        if (this.OperandStr.EndsWith("]"))
                        {
                            if (this.Operands_Name[3].Equals("lsl"))
                            {
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsl(this.Operands[2], this.Operands[3]) + k));
                                    // Src
                                    List<String> item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }
                            }
                            else if (this.Operands_Name[3].Equals("lsr"))
                            {
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsr(this.Operands[2], this.Operands[3]) + k));
                                    // Src
                                    List<String> item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }
                            }
                            else if (this.Operands_Name[3].Equals("asr"))
                            {
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.asr((int)this.Operands[2], this.Operands[3]) + k));
                                    // Src
                                    List<String> item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this.GetLine());
                            }
                        }
                        // STR{type}{cond} Rt, [Rn, +/-Rm, shift]!       ; pre-indexed
                        else if (this.OperandStr.EndsWith("!"))
                        {
                            if (this.Operands_Name[3].Equals("lsl"))
                            {
                                List<String> item;
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst 1
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsl(this.Operands[2], this.Operands[3]) + k));
                                    // Src 1
                                    item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else if (this.Operands_Name[3].Equals("lsr"))
                            {
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.lsr(this.Operands[2], this.Operands[3]) + k));
                                    // Src
                                    List<String> item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }
                            }
                            else if (this.Operands_Name[3].Equals("asr"))
                            {
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + this.asr((int)this.Operands[2], this.Operands[3]) + k));
                                    // Src
                                    List<String> item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }
                            }
                            else
                            {
                                MessageBox.Show(this.GetLine());
                            }
                        }
                        // STR{type}{cond} Rt, [Rn], +/-Rm, shift        ; post-indexed
                        else
                        {
                            if (this.Operands_Name[3].Equals("lsl")
                                || this.Operands_Name[3].Equals("lsr")
                                || this.Operands_Name[3].Equals("asr"))
                            {
                                List<String> item;
                                for ( int k = 0 ;
                                    byte_sign != 0 ;
                                    k++, byte_sign = byte_sign >> 1 ) {
                                    // Dst 1
                                    this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[1] + k));
                                    // Src 1
                                    item = new List<String>();
                                    item.Add(this.Operands_Name[0]);
                                    this.Operands_Src.Add(item);
                                }

                                // Dst 2
                                this.Operands_Dst.Add(this.Operands_Name[1]);
                                // Src 2
                                item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                            else
                            {
                                MessageBox.Show(this.GetLine());
                            }
                        }
                    }
                    else if (this.Mnemonic.Length > 3)
                    {
                        if (this.Mnemonic[3] == 'd')
                        {
                            if (this.Operand_Cnt == 3)
                            {
                                // STRD{cond}      Rt, Rt2, [Rn]                 ; immediate offset, doubleword
                                if (!Char.IsDigit(this.Operands_Name[2][0]) && this.Operands_Name[2][0] != '-')
                                {
                                    List<String> item;
                                    for ( int k = 0 ;
                                        byte_sign != 0 ;
                                        k++, byte_sign = byte_sign >> 1 ) {
                                        // Dst 1
                                        this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + k));
                                        // Src 1
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[0]);
                                        this.Operands_Src.Add(item);
                                    }

                                    for ( int k = 0 ;
                                        byte_sign != 0 ;
                                        k++, byte_sign = byte_sign >> 1 ) {
                                        // Dst 2
                                        this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + k));
                                        // Src 2
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[1]);
                                        this.Operands_Src.Add(item);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this.GetLine());
                                }
                            }

                            else if (this.Operand_Cnt == 4)
                            {
                                if (!Char.IsDigit(this.Operands_Name[2][0]) && this.Operands_Name[2][0] != '-')
                                {
                                    // STRD{cond}      Rt, Rt2, [Rn, +/-Rm]          ; register offset, doubleword
                                    if (this.OperandStr.EndsWith("]"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }
                                    }

                                    // STRD{cond}      Rt, Rt2, [Rn, +/-Rm]!         ; pre-indexed, doubleword
                                    else if (this.OperandStr.EndsWith("!"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }

                                    // STRD{cond}      Rt, Rt2, [Rn], +/-Rm          ; post-indexed, doubleword
                                    else
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                }
                                else
                                {
                                    // STRD{cond}      Rt, Rt2, [Rn, #offset]        ; immediate offset, doubleword
                                    if (this.OperandStr.EndsWith("]"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }
                                    }

                                    // STRD{cond}      Rt, Rt2, [Rn, #offset]!       ; pre-indexed, doubleword
                                    else if (this.OperandStr.EndsWith("!"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.Operands[3] + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);
                                    }

                                    // STRD{cond}      Rt, Rt2, [Rn], #offset        ; post-indexed, doubleword
                                    else
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        this.Operands_Src.Add(item);
                                    }
                                }
                            }

                            else if (this.Operand_Cnt == 6)
                            {
                                // STRD{cond}      Rt, Rt2, [Rn, +/-Rm, shift]   ; register offset, doubleword
                                if (this.OperandStr.EndsWith("]"))
                                {
                                    if (this.Operands_Name[4].Equals("lsl"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsl(this.Operands[3], this.Operands[4]) + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsl(this.Operands[3], this.Operands[4]) + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }
                                    }
                                    else if (this.Operands_Name[4].Equals("lsr"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsr(this.Operands[3], this.Operands[4]) + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsr(this.Operands[3], this.Operands[4]) + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }
                                    }
                                    else if (this.Operands_Name[4].Equals("asr"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.asr((int)this.Operands[3], this.Operands[4]) + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.asr((int)this.Operands[3], this.Operands[4]) + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(this.GetLine());
                                    }
                                }

                                // STRD{cond}      Rt, Rt2, [Rn, +/-Rm, shift]!  ; pre-indexed, doubleword
                                else if (this.OperandStr.EndsWith("!"))
                                {
                                    if (this.Operands_Name[4].Equals("lsl"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsl(this.Operands[3], this.Operands[4]) + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsl(this.Operands[3], this.Operands[4]) + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else if (this.Operands_Name[4].Equals("lsr"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsr(this.Operands[3], this.Operands[4]) + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.lsr(this.Operands[3], this.Operands[4]) + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else if (this.Operands_Name[4].Equals("asr"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.asr((int)this.Operands[3], this.Operands[4]) + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + this.asr((int)this.Operands[3], this.Operands[4]) + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this.GetLine());
                                    }
                                }

                                // STRD{cond}      Rt, Rt2, [Rn], +/-Rm, shift   ; post-indexed, doubleword
                                else
                                {
                                    if (this.Operands_Name[4].Equals("lsl")
                                        || this.Operands_Name[4].Equals("lsr")
                                        || this.Operands_Name[4].Equals("asr"))
                                    {
                                        List<String> item;
                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 1
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + k));
                                            // Src 1
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[0]);
                                            this.Operands_Src.Add(item);
                                        }

                                        for ( int k = 0 ;
                                            byte_sign != 0 ;
                                            k++, byte_sign = byte_sign >> 1 ) {
                                            // Dst 2
                                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + 0x4 + k));
                                            // Src 2
                                            item = new List<String>();
                                            item.Add(this.Operands_Name[1]);
                                            this.Operands_Src.Add(item);
                                        }

                                        // Dst 3
                                        this.Operands_Dst.Add(this.Operands_Name[2]);
                                        // Src 3
                                        item = new List<String>();
                                        item.Add(this.Operands_Name[2]);
                                        item.Add(this.Operands_Name[3]);
                                        this.Operands_Src.Add(item);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this.GetLine());
                                    }
                                }
                            }

                            else
                            {
                                MessageBox.Show("00LDRD\n" + this.GetLine());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("01STRD_adr\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("adr"))
                {
                    // ADR{cond}{.W} Rd,label
                    // Dst 1
                    this.Operands_Dst.Add(this.Operands_Name[0]);
                    // Src 1
                    List<String> item = new List<String>();
                    item.Add("pc");
                    this.Operands_Src.Add(item);
                }
                else if (this.Mnemonic.Contains("pldw"))
                {
                    // PLDW{cond} [Rn {, #offset}]
                    // PLDW{cond} [Rn, +/-Rm {, shift}]
                    // PLDW{cond} label

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("pld"))
                {
                    // PLD{cond} [Rn {, #offset}]
                    // PLD{cond} [Rn, +/-Rm {, shift}]
                    // PLD{cond} label

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("pli"))
                {
                    // PLI{cond} [Rn {, #offset}]
                    // PLI{cond} [Rn, +/-Rm {, shift}]
                    // PLI{cond} label

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("ldm"))
                {
                    // LDM{addr_mode}{cond} Rn!, reglist{^}
                    if (this.Operands_Name[0].EndsWith("!"))
                    {
                        this.Operands_Name[0] = this.Operands_Name[0].Substring(0,2);
                        //this.Operands_Name[0] = this.Operands_Name[0].TrimEnd('!');

                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        this.Operands_Src.Add(item);

                        //SrcDstParsingCheck();
                    }
                    // LDM{addr_mode}{cond} Rn, reglist{^}
                    if (this.Mnemonic.Contains("ldmdb"))
                    {
                        for (int i = 1; i < this.Operand_Cnt; i++)
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[i]);
                            // Src
                            List<String> item = new List<String>();
                            uint byte_check = byte_sign;
                            for ( int k = 0 ;
                                byte_check != 0 ;
                                k++, byte_check = byte_check >> 1 ) {
                                item.Add(String.Format("0x{0,8:X8}", this.Operands[0] - (4 * i) + k));
                            }
                            this.Operands_Src.Add(item);
                        }
                    }
                    else if (this.Mnemonic.Contains("ldmda"))
                    {
                        for (int i = 1; i < this.Operand_Cnt; i++)
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[i]);
                            // Src
                            List<String> item = new List<String>();
                            uint byte_check = byte_sign;
                            for ( int k = 0 ;
                                byte_check != 0 ;
                                k++, byte_check = byte_check >> 1 ) {
                                item.Add(String.Format("0x{0,8:X8}", this.Operands[0] - (4 * (i - 1)) + k));
                            }
                            this.Operands_Src.Add(item);
                        }
                    }
                    else if (this.Mnemonic.Contains("ldmib"))
                    {
                        for (int i = 1; i < this.Operand_Cnt; i++)
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[i]);
                            // Src
                            List<String> item = new List<String>();
                            uint byte_check = byte_sign;
                            for ( int k = 0 ;
                                byte_check != 0 ;
                                k++, byte_check = byte_check >> 1 ) {
                                item.Add(String.Format("0x{0,8:X8}", this.Operands[0] + (4 * i) + k));
                            }
                            this.Operands_Src.Add(item);
                        }
                    }
                    // ldmia or default
                    else
                    {
                        for (int i = 1; i < this.Operand_Cnt; i++)
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[i]);
                            // Src
                            List<String> item = new List<String>();
                            uint byte_check = byte_sign;
                            for ( int k = 0 ;
                                byte_check != 0 ;
                                k++, byte_check = byte_check >> 1 ) {
                                item.Add(String.Format("0x{0,8:X8}", this.Operands[0] + (4 * (i - 1)) + k));
                            }
                            this.Operands_Src.Add(item);
                        }
                    }
                }
                else if (this.Mnemonic.Contains("stm"))
                {
                    // STM{addr_mode}{cond} Rn!, reglist{^}
                    if (this.Operands_Name[0].EndsWith("!"))
                    {
                        this.Operands_Name[0] = this.Operands_Name[0].Substring(0, 2);
                        //this.Operands_Name[0] = this.Operands_Name[0].TrimEnd('!');

                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        this.Operands_Src.Add(item);
                    }
                    // STM{addr_mode}{cond} Rn, reglist{^}
                    if (this.Mnemonic.Contains("stmdb"))
                    {
                        for (int i = 1; i < this.Operand_Cnt; i++)
                        {
                            uint byte_check = byte_sign;
                            for ( int k = 0 ;
                                byte_check != 0 ;
                                k++, byte_check = byte_check >> 1 ) {
                                // Dst
                                this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[0] - (4 * i) + k));
                                // Src
                                List<String> item = new List<String>();
                                item.Add(this.Operands_Name[i]);
                                this.Operands_Src.Add(item);
                            }
                        }
                    }
                    else if (this.Mnemonic.Contains("stmda"))
                    {
                        for (int i = 1; i < this.Operand_Cnt; i++)
                        {
                            uint byte_check = byte_sign;
                            for ( int k = 0 ;
                                byte_check != 0 ;
                                k++, byte_check = byte_check >> 1 ) {
                                // Dst
                                this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[0] - (4 * (i - 1)) + k));
                                // Src
                                List<String> item = new List<String>();
                                item.Add(this.Operands_Name[i]);
                                this.Operands_Src.Add(item);
                            }
                        }
                    }
                    else if (this.Mnemonic.Contains("stmib"))
                    {
                        for (int i = 1; i < this.Operand_Cnt; i++)
                        {
                            uint byte_check = byte_sign;
                            for ( int k = 0 ;
                                byte_check != 0 ;
                                k++, byte_check = byte_check >> 1 ) {
                                // Dst
                                this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[0] + (4 * i) + k));
                                // Src
                                List<String> item = new List<String>();
                                item.Add(this.Operands_Name[i]);
                                this.Operands_Src.Add(item);
                            }
                        }
                    }
                    // stmia or default
                    else
                    {
                        for (int i = 1; i < this.Operand_Cnt; i++)
                        {
                            uint byte_check = byte_sign;
                            for ( int k = 0 ;
                                byte_check != 0 ;
                                k++, byte_check = byte_check >> 1 ) {
                                // Dst
                                this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[0] + (4 * (i - 1)) + k));
                                // Src
                                List<String> item = new List<String>();
                                item.Add(this.Operands_Name[i]);
                                this.Operands_Src.Add(item);
                            }
                        }
                    }
                }
                else if (this.Mnemonic.Contains("push"))
                {
                    // PUSH{cond} reglist // =STMDB
                    for (int i = 0; i < this.Operand_Cnt; i++)
                    {
                        uint byte_check = byte_sign;
                        for ( int k = 0 ;
                            byte_check != 0 ;
                            k++, byte_check = byte_check >> 1 ) {
                            // Dst
                            this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[12] - (4 * (Operand_Cnt - i)) + k));

                            // Src
                            List<String> item = new List<String>();
                            item.Add(this.Operands_Name[i]);
                            this.Operands_Src.Add(item);
                        }
                    }
                }
                else if (this.Mnemonic.Contains("pop"))
                {
                    // POP{cond} reglist // =LDMIA
                    for (int i = 0; i < this.Operand_Cnt; i++)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[i]);

                        // Src
                        List<String> item = new List<String>();
                        uint byte_check = byte_sign;
                        for (int k = 0;
                            byte_check != 0;
                            k++, byte_check = byte_check >> 1) {
                            item.Add(String.Format("0x{0,8:X8}", this.Operands[12] + (4 * i) + k));
                        }
                        this.Operands_Src.Add(item);
                    }
                }
                else if (this.Mnemonic.Contains("clrex"))
                {
                    // CLREX{cond}

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("swp"))
                {
                    // SWP{cond} Rt, Rt2, [Rn]
                    // SWPB{cond} Rt, Rt2, [Rn]

                    // Dst 1
                    this.Operands_Dst.Add(this.Operands_Name[0]);
                    // Src 1
                    List<String> item = new List<String>();
                    for ( int k = 0 ;
                        byte_sign != 0 ;
                        k++, byte_sign = byte_sign >> 1 ) {
                        item.Add(String.Format("0x{0,8:X8}", this.Operands[2] + k));
                    }
                    item.Add(this.Operands_Name[2]);
                    this.Operands_Src.Add(item);

                    for ( int k = 0 ;
                        byte_sign != 0 ;
                        k++, byte_sign = byte_sign >> 1 ) {
                        // Dst 2
                        this.Operands_Dst.Add(String.Format("0x{0,8:X8}", this.Operands[2] + k));
                        // Src 2
                        item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }
                }
                else if (this.Mnemonic.Contains("add")
                    || this.Mnemonic.Contains("sub")
                    || this.Mnemonic.Contains("rsb")
                    || this.Mnemonic.Contains("adc")
                    || this.Mnemonic.Contains("sbc")
                    || this.Mnemonic.Contains("rsc")
                    || this.Mnemonic.Contains("and")
                    || this.Mnemonic.Contains("orr")
                    || this.Mnemonic.Contains("eor")
                    || this.Mnemonic.Contains("bic")
                    || this.Mnemonic.Contains("orn")
                    || this.Mnemonic.Contains("asr")
                    || this.Mnemonic.Contains("lsl")
                    || this.Mnemonic.Contains("lsr")
                    || this.Mnemonic.Contains("ror")
                    )
                {
                    if (this.Operand_Cnt == 2)
                    {
                        // ADD{S}{cond} Rn, Rm
                        // QADD{cond} Rm, Rn
                        // QDADD{cond} Rm, Rn
                        // SUB{S}{cond} Rn, Rm
                        // QSUB{cond} Rm, Rn
                        // QDSUB{cond} Rm, Rn
                        // RSB{S}{cond} Rn, Rm
                        // ADC{S}{cond} Rn, Rm
                        // SBC{S}{cond} Rn, Rm
                        // RSC{S}{cond} Rn, Rm

                        // 2nd operand is register
                        if (!Char.IsDigit(this.Operands_Name[1][0]) && this.Operands_Name[1][0] != '-')
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[0]);
                            // Src
                            List<String> item = new List<String>();
                            item.Add(this.Operands_Name[0]);
                            item.Add(this.Operands_Name[1]);
                            this.Operands_Src.Add(item);
                        }

                        // ADD{cond} Rn, #imm12
                        // SUB{cond} Rn, #imm12
                        // RSB{cond} Rn, #imm12
                        // ADC{cond} Rn, #imm12
                        // SBC{cond} Rn, #imm12
                        // RSC{cond} Rn, #imm12

                        // 2nd operand is #
                        else
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[0]);
                            // Src
                            List<String> item = new List<String>();
                            item.Add(this.Operands_Name[0]);
                            this.Operands_Src.Add(item);
                        }
                    }
                    else if (this.Operand_Cnt == 3)
                    {
                        // ADD{S}{cond} Rd, Rn, Rm
                        // QADD{cond} Rd, Rm, Rn
                        // QDADD{cond} Rd, Rm, Rn
                        // SUB{S}{cond} Rd, Rn, Rm
                        // QSUB{cond} Rd, Rm, Rn
                        // QDSUB{cond} Rd, Rm, Rn
                        // RSB{S}{cond} Rd, Rn, Rm
                        // ADC{S}{cond} Rd, Rn, Rm
                        // SBC{S}{cond} Rd, Rn, Rm
                        // RSC{S}{cond} Rd, Rn, Rm
                        // AND{S}{cond} Rd, Rn, Rm
                        // ORR{S}{cond} Rd, Rn, Rm
                        // EOR{S}{cond} Rd, Rn, Rm
                        // BIC{S}{cond} Rd, Rn, Rm
                        // ORN{S}{cond} Rd, Rn, Rm
                        // ASR{S}{cond} Rd, Rm, Rs
                        // LSL{S}{cond} Rd, Rm, Rs
                        // LSR{S}{cond} Rd, Rm, Rs
                        // ROR{S}{cond} Rd, Rm, Rs

                        // 3rd operand is register
                        if (!Char.IsDigit(this.Operands_Name[2][0]) && this.Operands_Name[2][0] != '-')
                        {
                            if (this.Operands_Name[1] == this.Operands_Name[2])
                            {
                                // Dst
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src
                                List<String> item = new List<String>();
                                item.Add("END");
                                this.Operands_Src.Add(item);
                            }
                            else
                            {
                                // Dst
                                this.Operands_Dst.Add(this.Operands_Name[0]);
                                // Src
                                List<String> item = new List<String>();
                                item.Add(this.Operands_Name[1]);
                                item.Add(this.Operands_Name[2]);
                                this.Operands_Src.Add(item);
                            }
                        }

                        // ADD{cond} Rd, Rn, #imm12
                        // SUB{cond} Rd, Rn, #imm12
                        // RSB{cond} Rd, Rn, #imm12
                        // ADC{cond} Rd, Rn, #imm12
                        // SBC{cond} Rd, Rn, #imm12
                        // RSC{cond} Rd, Rn, #imm12

                        // 3rd operand is #
                        else
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[0]);
                            // Src
                            List<String> item = new List<String>();
                            item.Add(this.Operands_Name[1]);
                            this.Operands_Src.Add(item);
                        }
                    }
                    else if (this.Operand_Cnt == 4)
                    {
                        // ADD{S}{cond} Rn, Rm, shift
                        // SUB{S}{cond} Rn, Rm, shift
                        // RSB{S}{cond} Rn, Rm, shift
                        // ADC{S}{cond} Rn, Rm, shift
                        // SBC{S}{cond} Rn, Rm, shift
                        // RSC{S}{cond} Rn, Rm, shift
                        // ASR{S}{cond} Rd, Rm, shift
                        // LSL{S}{cond} Rd, Rm, shift
                        // LSR{S}{cond} Rd, Rm, shift
                        // ROR{S}{cond} Rd, Rm, shift

                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }
                    else if (this.Operand_Cnt == 5)
                    {
                        // ADD{S}{cond} Rd, Rn, Rm, shift
                        // SUB{S}{cond} Rd, Rn, Rm, shift
                        // RSB{S}{cond} Rd, Rn, Rm, shift
                        // ADC{S}{cond} Rd, Rn, Rm, shift
                        // SBC{S}{cond} Rd, Rn, Rm, shift
                        // RSC{S}{cond} Rd, Rn, Rm, shift
                        // AND{S}{cond} Rd, Rn, Rm, shift
                        // ORR{S}{cond} Rd, Rn, Rm, shift
                        // EOR{S}{cond} Rd, Rn, Rm, shift
                        // BIC{S}{cond} Rd, Rn, Rm, shift
                        // ORN{S}{cond} Rd, Rn, Rm, shift

                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }
                    else
                    {
                        // Error
                        MessageBox.Show("00CLZ\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("rfe"))
                {
                    // RFE{addr_mode}{cond} Rn{!}

                    MessageBox.Show("00RFE\n" + this.GetLine());
                }
                else if (this.Mnemonic.Contains("srs"))
                {
                    // SRS{addr_mode}{cond} sp{!}, #modenum
                    // SRS{addr_mode}{cond} #modenum{!}        ; This is a pre-UAL syntax

                    MessageBox.Show("00SRS\n" + this.GetLine());
                }
                else if (this.Mnemonic.Contains("clz"))
                {
                    // CLZ{cond} Rd, Rm

                    // Dst
                    this.Operands_Dst.Add(this.Operands_Name[0]);
                    // Src
                    List<String> item = new List<String>();
                    item.Add(this.Operands_Name[1]);
                    this.Operands_Src.Add(item);
                }
                else if (this.Mnemonic.Contains("cmp"))
                {
                    // CMP{cond} Rn, Operand2

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("cmn"))
                {
                    // CMN{cond} Rn, Operand2

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("mov")
                    || this.Mnemonic.Contains("mvn"))
                {
                    // MOV{S}{cond} Rd, Rm
                    // MOV{S}{cond} Rd, Rm, shift
                    // MVN{S}{cond} Rd, Rm
                    // MVN{S}{cond} Rd, Rm, shift

                    // 2nd operand is register
                    if (!Char.IsDigit(this.Operands_Name[1][0]) && this.Operands_Name[1][0] != '-')
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    // MOV{cond} Rd, #imm16
                    // MOVT{cond} Rd, #immed_16
                    else
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add("END");
                        this.Operands_Src.Add(item);
                    }
                }
                else if (this.Mnemonic.Contains("tst"))
                {
                    // TST{cond} Rn, Operand2
                }
                else if (this.Mnemonic.Contains("teq"))
                {
                    // TEQ{cond} Rn, Operand2
                }
                else if (this.Mnemonic.Contains("sel"))
                {
                    // SEL{cond} Rn, Rm
                    if (this.Operand_Cnt == 2)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    // SEL{cond} Rd, Rn, Rm
                    else if (this.Operand_Cnt == 3)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }

                    else
                    {
                        MessageBox.Show("00SEL\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("rev")
                    || this.Mnemonic.Contains("rbit")
                    || this.Mnemonic.Contains("rrx")
                    )
                {
                    // REV{cond} Rd, Rn
                    // REV16{cond} Rd, Rn
                    // REVSH{cond} Rd, Rn
                    // RBIT{cond} Rd, Rn
                    // RRX{S}{cond} Rd, Rm

                    // Dst
                    this.Operands_Dst.Add(this.Operands_Name[0]);
                    // Src
                    List<String> item = new List<String>();
                    item.Add(this.Operands_Name[1]);
                    this.Operands_Src.Add(item);
                }
                else if (this.Mnemonic.Contains("div"))
                {
                    // SDIV{cond} Rn, Rm
                    // UDIV{cond} Rn, Rm
                    if (this.Operand_Cnt == 2)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }
                    // SDIV{cond} Rd, Rn, Rm
                    // UDIV{cond} Rd, Rn, Rm
                    else if (this.Operand_Cnt == 3)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("00MUL\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("mul"))
                {
                    // MUL{S}{cond} Rn, Rm
                    // SMUL<x><y>{cond} Rn, Rm
                    // SMULW<y>{cond} Rn, Rm
                    // SMMUL{R}{cond} Rn, Rm
                    if (this.Operand_Cnt == 2)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    // MUL{S}{cond} Rd, Rn, Rm
                    // SMUL<x><y>{cond} Rd, Rn, Rm
                    // SMULW<y>{cond} Rd, Rn, Rm
                    // SMMUL{R}{cond} Rd, Rn, Rm
                    else if (this.Operand_Cnt == 3)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }

                    // UMULL{S}{cond} RdLo, RdHi, Rn, Rm
                    // SMULL{S}{cond} RdLo, RdHi, Rn, Rm
                    else if (this.Operand_Cnt == 4)
                    {
                        // Dst 1
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src 1
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);

                        // Dst 2
                        this.Operands_Dst.Add(this.Operands_Name[1]);
                        // Src 2
                        item = new List<String>();
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);
                    }

                    else
                    {
                        MessageBox.Show("00MLA\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("mla"))
                {
                    // UMLAL{S}{cond} RdLo, RdHi, Rn, Rm
                    // SMLAL{S}{cond} RdLo, RdHi, Rn, Rm
                    // SMLAL<x><y>{cond} RdLo, RdHi, Rn, Rm
                    // SMLALD{X}{cond} RdLo, RdHi, Rn, Rm
                    if (this.Mnemonic.Contains("mlal"))
                    {
                        // Dst 1
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src 1
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);

                        // Dst 2
                        this.Operands_Dst.Add(this.Operands_Name[1]);
                        // Src 2
                        item = new List<String>();
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);
                    }

                    // MLA{S}{cond} Rd, Rn, Rm, Ra
                    // SMLA<x><y>{cond} Rd, Rn, Rm, Ra
                    // SMLAW<y>{cond} Rd, Rn, Rm, Ra
                    // SMMLA{R}{cond} Rd, Rn, Rm, Ra
                    // SMLAD{X}{cond} Rd, Rn, Rm, Ra
                    else
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);
                    }

                }
                else if (this.Mnemonic.Contains("mls"))
                {
                    // SMLSLD{X}{cond} RdLo, RdHi, Rn, Rm
                    if (this.Mnemonic.Contains("mlal"))
                    {
                        // Dst 1
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src 1
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);

                        // Dst 2
                        this.Operands_Dst.Add(this.Operands_Name[1]);
                        // Src 2
                        item = new List<String>();
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);
                    }

                    // MLS{cond} Rd, Rn, Rm, Ra
                    // SMMLS{R}{cond} Rd, Rn, Rm, Ra
                    // SMLSD{X}{cond} Rd, Rn, Rm, Ra
                    else
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);
                    }
                }
                else if (this.Mnemonic.Contains("smuad")
                    || this.Mnemonic.Contains("smusd"))
                {
                    // SMUAD{X}{cond} Rn, Rm
                    // SMUSD{X}{cond} Rn, Rm
                    if (this.Operand_Cnt == 2)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    // SMUAD{X}{cond} Rd, Rn, Rm
                    // SMUSD{X}{cond} Rd, Rn, Rm
                    else if (this.Operand_Cnt == 3)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }

                    else
                    {
                        MessageBox.Show("00UMAAL\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("umaal"))
                {
                    // UMAAL{cond} RdLo, RdHi, Rn, Rm

                    // Dst 1
                    this.Operands_Dst.Add(this.Operands_Name[0]);
                    // Src 1
                    List<String> item = new List<String>();
                    item.Add(this.Operands_Name[2]);
                    item.Add(this.Operands_Name[3]);
                    this.Operands_Src.Add(item);

                    // Dst 2
                    this.Operands_Dst.Add(this.Operands_Name[1]);
                    // Src 2
                    item = new List<String>();
                    item.Add(this.Operands_Name[2]);
                    item.Add(this.Operands_Name[3]);
                    this.Operands_Src.Add(item);
                }
                else if (this.Mnemonic.Contains("mia"))
                {
                    // MIA{cond} Acc, Rn, Rm
                    // MIAPH{cond} Acc, Rn, Rm
                    // MIA<x><y>{cond} Acc, Rn, Rm

                    MessageBox.Show("00MIA\n" + this.GetLine());
                }
                else if (this.Mnemonic.Contains("sat"))
                {
                    // SSAT{cond} Rd, #sat, Rm
                    // USAT{cond} Rd, #sat, Rm
                    // SSAT16{cond} Rd, #sat, Rn
                    // USAT16{cond} Rd, #sat, Rn
                    // SSAT{cond} Rd, #sat, Rm, shift
                    // USAT{cond} Rd, #sat, Rm, shift

                    // Dst
                    this.Operands_Dst.Add(this.Operands_Name[0]);
                    // Src
                    List<String> item = new List<String>();
                    item.Add(this.Operands_Name[2]);
                    this.Operands_Src.Add(item);
                }
                else if (this.Mnemonic.Contains("sad"))
                {
                    // USAD8{cond} Rn, Rm
                    if (this.Operand_Cnt == 2)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    // USAD8{cond} Rd, Rn, Rm
                    else if (this.Operand_Cnt == 3)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }

                    // USADA8{cond} Rd, Rn, Rm, Ra
                    else if (this.Operand_Cnt == 4)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        item.Add(this.Operands_Name[3]);
                        this.Operands_Src.Add(item);
                    }

                    else
                    {
                        MessageBox.Show("00BF\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("bf"))
                {
                    // BFC{cond} Rd, #lsb, #width
                    if (this.Operand_Cnt == 3)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        this.Operands_Src.Add(item);
                    }

                    // BFI{cond} Rd, Rn, #lsb, #width
                    // SBFX{cond} Rd, Rn, #lsb, #width
                    // UBFX{cond} Rd, Rn, #lsb, #width
                    else if (this.Operand_Cnt == 4)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    else
                    {
                        MessageBox.Show("00XT\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("xt"))
                {
                    // SXT<extend>{cond}  Rm
                    // UXT<extend>{cond}  Rm
                    if (this.Operand_Cnt == 1)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        this.Operands_Src.Add(item);
                    }

                    // SXTA<extend>{cond} Rn, Rm
                    // UXTA<extend>{cond} Rn, Rm
                    // SXT<extend>{cond}  Rd, Rm
                    // UXT<extend>{cond}  Rd, Rm
                    else if (this.Operand_Cnt == 2)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    else if (this.Operand_Cnt == 3)
                    {
                        // SXT<extend>{cond}  Rm, rotation
                        // UXT<extend>{cond}  Rm, rotation
                        if (this.Operands_Name[1].Equals("ror"))
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[0]);
                            // Src
                            List<String> item = new List<String>();
                            item.Add(this.Operands_Name[0]);
                            this.Operands_Src.Add(item);
                        }

                        // SXTA<extend>{cond} Rd, Rn, Rm
                        // UXTA<extend>{cond} Rd, Rn, Rm
                        else
                        {
                            // Dst
                            this.Operands_Dst.Add(this.Operands_Name[0]);
                            // Src
                            List<String> item = new List<String>();
                            item.Add(this.Operands_Name[0]);
                            item.Add(this.Operands_Name[1]);
                            item.Add(this.Operands_Name[2]);
                            this.Operands_Src.Add(item);
                        }
                    }

                    // SXTA<extend>{cond} Rn, Rm, rotation
                    // UXTA<extend>{cond} Rn, Rm, rotation
                    // SXT<extend>{cond}  Rd, Rm, rotation
                    // UXT<extend>{cond}  Rd, Rm, rotation
                    else if (this.Operand_Cnt == 4)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    // SXTA<extend>{cond} Rd, Rn, Rm, rotation
                    // UXTA<extend>{cond} Rd, Rn, Rm, rotation
                    else if (this.Operand_Cnt == 5)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }

                    else
                    {
                        MessageBox.Show("00PKH" + this.GetLine());
                    }
                }
                else if (this.Mnemonic.Contains("pkh"))
                {
                    // PKHBT{cond} Rn, Rm
                    // PKHTB{cond} Rn, Rm
                    if (this.Operand_Cnt == 2)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    // PKHBT{cond} Rd, Rn, Rm
                    // PKHTB{cond} Rd, Rn, Rm
                    else if (this.Operand_Cnt == 3)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }

                    // PKHBT{cond} Rn, Rm, LSL #leftshift
                    // PKHTB{cond} Rn, Rm, ASR #rightshift
                    else if (this.Operand_Cnt == 4)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        this.Operands_Src.Add(item);
                    }

                    // PKHBT{cond} Rd, Rn, Rm, LSL #leftshift
                    // PKHTB{cond} Rd, Rn, Rm, ASR #rightshift
                    else if (this.Operand_Cnt == 5)
                    {
                        // Dst
                        this.Operands_Dst.Add(this.Operands_Name[0]);
                        // Src
                        List<String> item = new List<String>();
                        item.Add(this.Operands_Name[0]);
                        item.Add(this.Operands_Name[1]);
                        item.Add(this.Operands_Name[2]);
                        this.Operands_Src.Add(item);
                    }

                    else
                    {
                        MessageBox.Show("00B\n" + this.GetLine());
                    }
                }
                else if (this.Mnemonic[0].Equals('b'))
                {
                    // BL{cond}{.W} label
                    // BLX{cond} Rm
                    if (this.Mnemonic.Contains("bl"))
                    {
                        // Link Register
                    }

                    // B{cond}{.W} label
                    // BLX{cond}{.W} label
                    // BX{cond} Rm
                    // BXJ{cond} Rm
                }
                else if (this.Mnemonic.Contains("cb"))
                {
                    // CBZ Rn, label
                    // CBNZ Rn, label

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("tb"))
                {
                    // TBB [Rn, Rm]
                    // TBH [Rn, Rm, LSL #1]

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("it"))
                {
                    // IT{x{y{z}}} {cond}

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("cdp"))
                {
                    // CDP{cond} coproc, #opcode1, CRd, CRn, CRm{, #opcode2}
                    // CDP2{cond} coproc, #opcode1, CRd, CRn, CRm{, #opcode2}

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("mcr"))
                {
                    // MCR{cond} coproc, #opcode1, Rt, CRn, CRm{, #opcode2}
                    // MCR2{cond} coproc, #opcode1, Rt, CRn, CRm{, #opcode2}
                    // MCRR{cond} coproc, #opcode3, Rt, Rt2, CRm
                    // MCRR2{cond} coproc, #opcode3, Rt, Rt2, CRm

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("mrc"))
                {
                    // MRC{cond} coproc, #opcode1, Rt, CRn, CRm{, #opcode2}
                    // MRC2{cond} coproc, #opcode1, Rt, CRn, CRm{, #opcode2}
                    // MRRC{cond} coproc, #opcode3, Rt, Rt2, CRm
                    // MRRC2{cond} coproc, #opcode3, Rt, Rt2, CRm

                    //MessageBox.Show("00MRC\n" + this.GetLine());
                }
                else if (this.Mnemonic.Contains("ldc"))
                {
                    // LDC{L}{cond} coproc, CRd, [Rn]
                    // LDC{L}{cond} coproc, CRd, [Rn, #{-}offset]{!}
                    // LDC{L}{cond} coproc, CRd, [Rn], #{-}offset
                    // LDC{L}{cond} coproc, CRd, label

                    MessageBox.Show("00STC\n" + this.GetLine());
                }
                else if (this.Mnemonic.Contains("stc"))
                {
                    // STC{L}{cond} coproc, CRd, [Rn]
                    // STC{L}{cond} coproc, CRd, [Rn, #{-}offset]{!}
                    // STC{L}{cond} coproc, CRd, [Rn], #{-}offset
                    // STC{L}{cond} coproc, CRd, label

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("mrs"))
                {
                    // MRS{cond} Rd, psr

                    MessageBox.Show("00MRS\n" + this.GetLine());
                }
                else if (this.Mnemonic.Contains("msr"))
                {
                    // MSR{cond} APSR_flags, #constant
                    // MSR{cond} APSR_flags, Rm
                    // MSR{cond} psr_fields, #constant
                    // MSR{cond} psr_fields, Rm

                    // MessageBox.Show(this.GetLine());
                }
                else if (this.Mnemonic.Contains("mar"))
                {
                    // MAR{cond} Acc, RdLo, RdHi

                    MessageBox.Show("00MAR\n" + this.GetLine());
                }
                else if (this.Mnemonic.Contains("mra"))
                {
                    // MRA{cond} RdLo, RdHi, Acc

                    MessageBox.Show("00MRA\n" + this.GetLine());
                }
                else if (this.Mnemonic.Contains("svc")
                    || this.Mnemonic.Contains("dmb")
                    || this.Mnemonic.Contains("nop")
                    || this.Mnemonic.Contains("udf"))
                {
                    // SVC{cond} #immed
                    // DMB{cond} {#option}
                    // NOP{cond}
                    // UDF       #num

                    // MessageBox.Show(this.GetLine());
                }
                else
                {
                    // BKPT #immed
                    // SMC{cond} #immed_4
                    // SETEND specifier
                    // SEV{cond}
                    // WFE{cond}
                    // WFI{cond}
                    // YIELD{cond}
                    // DBG{cond} {#option}
                    // DSB{cond} {#option}
                    // ISB{cond} {#option}
                    // CPSIE iflags{, #mode}
                    // CPSID iflags{, #mode}
                    // CPS #mode

                    //----- ThumbEE
                    // ENTERX
                    // LEAVEX
                    // CHKA Rn, Rm
                    // HB #HandlerID
                    // HBL #HandlerID
                    // HBP #immed, #HandlerID
                    // HBLP #immed, #HandlerID

                    //----- Undefined
                    // UND{cond}{.W} {#expr}

                    MessageBox.Show("00ETC\n" + this.GetLine());
                }
            }
        }

        /*
        [StructLayout(LayoutKind.Explicit, Size = 160)]
        public struct DATA
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 160)]
            [FieldOffset(0)]
            public Byte[] Contents;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 160)]
            [FieldOffset(0)]
            public UInt32 Count;
            [FieldOffset(4)]
            public UInt32 BitMask;
            [FieldOffset(8)]
            public UInt32 Address;
            [FieldOffset(12)]
            public UInt32 Opcode;

            [FieldOffset(16)]
            public Byte[] Mnemonic;
            [FieldOffset(32)]
            public Byte[] OperandStr;
            [FieldOffset(96)]
            public Byte[] Operands;
        }
        */
    }
}
