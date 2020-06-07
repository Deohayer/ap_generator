using System;
using System.Numerics;
using System.Text;

namespace Generator
{
    class Program
    {
        const string Filepath = "../../../../test_data/";
        // binary op
        const string FN_ADD = Filepath + "add.txt";
        const string FN_SUB = Filepath + "sub.txt";
        const string FN_MUL = Filepath + "mul.txt";
        const string FN_QUO = Filepath + "quo.txt";
        const string FN_REM = Filepath + "rem.txt";
        const string FN_AND = Filepath + "and.txt";
        const string FN_XOR = Filepath + "xor.txt";
        const string FN_OR = Filepath + "or.txt";
        const string FN_LESS = Filepath + "less.txt";
        const string FN_GREATER = Filepath + "greater.txt";
        const string FN_NLESS = Filepath + "nless.txt";
        const string FN_NGREATER = Filepath + "ngreater.txt";
        const string FN_EQUAL = Filepath + "equal.txt";
        const string FN_NEQUAL = Filepath + "nequal.txt";
        const string FN_RSHIFT = Filepath + "rshift.txt";
        const string FN_LSHIFT = Filepath + "lshift.txt";
        // unary op
        const string FN_NOT = Filepath + "not.txt";
        const string FN_DECV = Filepath + "decv.txt";
        const string FN_INCV = Filepath + "incv.txt";
        const string FN_VDEC = Filepath + "vdec.txt";
        const string FN_VINC = Filepath + "vinc.txt";
        const string FN_PLUS = Filepath + "plus.txt";
        const string FN_NEG = Filepath + "neg.txt";
        // values
        const string FN_VALUES = Filepath + "values.txt";
        const string FN_SVALUES = Filepath + "svalues.txt";

        const string Separator = "------------------------------";
        static Random r = new Random(4);
        const string digits = "0123456789";
        const int MinLength = 1;
        const int MiddleLength = 17;
        const int MaxLength = 301;
        const int TestsCount = 100;
        const int TinyValue = 500;

        delegate BigInteger Generator();
        delegate T BinaryOperation<T>(BigInteger left, BigInteger right);
        delegate BigInteger UnaryOperation(BigInteger val);
        delegate BigInteger ShiftOperation(BigInteger left, int right);

        int counter = 1;

        static void Main(string[] args)
        {
            Program p = new Program(); 
            System.IO.Directory.CreateDirectory(Filepath);
            p.GenerateBinaryOp(FN_ADD, (a, b) => a + b);
            p.GenerateBinaryOp(FN_SUB, (a, b) => a - b);
            p.GenerateBinaryOp(FN_MUL, (a, b) => a * b);
            p.GenerateBinaryOp(FN_QUO, (a, b) => a / b);
            p.GenerateBinaryOp(FN_REM, (a, b) => a % b);
            p.GenerateBinaryOp(FN_AND, (a, b) => a & b);
            p.GenerateBinaryOp(FN_XOR, (a, b) => a ^ b);
            p.GenerateBinaryOp(FN_OR, (a, b) => a | b);
            p.GenerateBinaryOp(FN_LESS, (a, b) => a < b);
            p.GenerateBinaryOp(FN_GREATER, (a, b) => a > b);
            p.GenerateBinaryOp(FN_NLESS, (a, b) => a >= b);
            p.GenerateBinaryOp(FN_NGREATER, (a, b) => a <= b);
            p.GenerateBinaryOp(FN_EQUAL, (a, b) => a == b);
            p.GenerateBinaryOp(FN_NEQUAL, (a, b) => a != b);
            p.GenerateShiftOp(FN_RSHIFT, (a, b) => a >> int.Parse(b.ToString()));
            p.GenerateShiftOp(FN_LSHIFT, (a, b) => a << int.Parse(b.ToString()));
            p.GenerateUnaryOp(FN_NOT, a => ~a);
            p.GenerateUnaryOp(FN_PLUS, a => +a);
            p.GenerateUnaryOp(FN_NEG, a => -a);
            p.GenerateUnaryOp(FN_DECV, a => --a);
            p.GenerateUnaryOp(FN_INCV, a => ++a);
            p.GenerateUnaryOp(FN_VDEC, a => a--);
            p.GenerateUnaryOp(FN_VINC, a => a++);
            p.GenerateValuesOp();
        }

        void GenerateBinaryOp<T>(string filename, BinaryOperation<T> op)
        {
            Trunc(filename);
            GenerateBinary(filename, TestsCount, GenerateShort, GenerateShort, op);
            GenerateBinary(filename, TestsCount, GenerateShort, GenerateLong, op);
            GenerateBinary(filename, TestsCount, GenerateLong, GenerateShort, op);
            GenerateBinary(filename, TestsCount, GenerateLong, GenerateLong, op);
            Reset();
        }

        void GenerateShiftOp(string filename, BinaryOperation<BigInteger> op)
        {
            Trunc(filename);
            GenerateBinary(filename, TestsCount, GenerateShort, GenerateTiny, op);
            GenerateBinary(filename, TestsCount, GenerateLong, GenerateTiny, op);
            Reset();
        }

        void GenerateUnaryOp(string filename, UnaryOperation op)
        {
            Trunc(filename);
            GenerateUnary(filename, TestsCount, GenerateShort, op);
            GenerateUnary(filename, TestsCount, GenerateLong, op);
            Reset();
        }

        void GenerateValuesOp()
        {
            Trunc(FN_VALUES);
            GenerateValues(FN_VALUES, TestsCount, GenerateShort);
            GenerateValues(FN_VALUES, TestsCount, GenerateLong);
            Reset();
            Trunc(FN_SVALUES);
            GenerateValues(FN_SVALUES, TestsCount, GenerateShort);
            Reset();
        }

        static void Trunc(string filename)
        {
            using (System.IO.StreamWriter writer =
                new System.IO.StreamWriter(filename))
            {
            }
        }

        static void PrintSeparator(System.IO.StreamWriter writer, int num)
        {
            writer.WriteLine($"{num}{Separator}");
        }

        void GenerateUnary(string filename, int count, Generator g, UnaryOperation op)
        {
            using (System.IO.StreamWriter writer =
                new System.IO.StreamWriter(filename, true))
            {
                PrintSeparator(writer, counter++);
                BigInteger value = BigInteger.Zero;
                writer.WriteLine(value);
                writer.WriteLine(op(value));
                for (int i = 2; i <= count; ++i)
                {
                    PrintSeparator(writer, counter++);
                    value = g();
                    writer.WriteLine(value);
                    writer.WriteLine(op(value));
                }
            }
        }

        void GenerateBinary<T>(string filename, int count, Generator lg, Generator rg, BinaryOperation<T> op)
        {
            using (System.IO.StreamWriter writer =
                new System.IO.StreamWriter(filename, true))
            {
                PrintSeparator(writer, counter++);
                BigInteger left = BigInteger.Zero;
                BigInteger right = rg();
                writer.WriteLine(left);
                writer.WriteLine(right);
                writer.WriteLine(op(left, right));
         
                PrintSeparator(writer, counter++);
                left = lg();
                right = BigInteger.Zero;
                writer.WriteLine(left);
                writer.WriteLine(right);
                try
                {
                    writer.WriteLine(op(left, right));
                }
                catch (DivideByZeroException)
                {
                    writer.WriteLine("Division");
                }

                for (int i = 3; i <= count; ++i)
                {
                    PrintSeparator(writer, counter++);
                    left = lg();
                    right = rg();
                    if (r.Next(1, 10) < 4 && (right > TinyValue || right < -TinyValue))
                    {
                        left = right;
                    }
                    writer.WriteLine(left);
                    writer.WriteLine(right);
                    writer.WriteLine(op(left, right));
                }
            }
        }

        void GenerateValues(string filename, int count, Generator g)
        {
            using (System.IO.StreamWriter writer =
                new System.IO.StreamWriter(filename, true))
            {
                PrintSeparator(writer, counter++);
                writer.WriteLine(BigInteger.Zero);
                for (int i = 2; i <= count; ++i)
                {
                    PrintSeparator(writer, counter++);
                    writer.WriteLine(g());
                }
            }
        }

        static BigInteger GenerateShort()
        {
            int count = r.Next(MinLength + 1, MiddleLength);
            int sign = r.Next(-1, 1);
            int i = 0;
            StringBuilder builder = new StringBuilder(count);
            if (sign < 0)
            {
                builder.Append('-');
                ++i;
            }
            builder.Append(digits[r.Next(1, 10)]);
            ++i;
            for (; i < count; ++i)
            {
                builder.Append(digits[r.Next(0, 10)]);
            }
            return BigInteger.Parse(builder.ToString());
        }

        static BigInteger GenerateLong()
        {
            int count = r.Next(MiddleLength + 1, MaxLength);
            int sign = r.Next(-1, 1);
            int i = 0;
            StringBuilder builder = new StringBuilder(count);
            if (sign < 0)
            {
                builder.Append('-');
                ++i;
            }
            builder.Append(digits[r.Next(1, 10)]);
            ++i;
            for (; i < count; ++i)
            {
                builder.Append(digits[r.Next(0, 10)]);
            }
            return BigInteger.Parse(builder.ToString());
        }

        static BigInteger GenerateTiny() // for shift operation
        {
            return r.Next(-TinyValue, TinyValue);
        }

        void Reset()
        {
            counter = 1;
        }
    }
}