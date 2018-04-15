using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using static System.Console;

namespace NovidadesCSharp7e8
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            await Task.Delay(500);
            WriteLine("Done");
            return 0;
        }

        public static int Somar(IEnumerable<object> valores)
        {
            var soma = 0;
            foreach (var item in valores)
            {
                if (item is int valor)
                    soma += valor;
                else if (item is IEnumerable<object> subItens)
                    soma += Somar(subItens);
                break;
            }

            return soma;
        }

        public static int Somar2(IEnumerable<object> valores)
        {
            var soma = 0;
            foreach (var item in valores)
            {
                switch (item)
                {
                    case 0:
                        break;
                    case int valor:
                        soma += valor;
                        break;
                    case DadosPercentuais dados:
                        soma += dados.Multiplicador * dados.Valor;
                        break;
                    case IEnumerable<object> subList when subList.Any():
                        soma += Somar(subList);
                        break;
                    case IEnumerable<object> subList:
                        break;
                    case null:
                        break;
                    default:
                        throw new InvalidOperationException("dado desconhecido");

                }
            }

            return soma;
        }

        public struct DadosPercentuais
        {
            public int Multiplicador { get; set; }
            public int Valor { get; set; }
        }

        public void Tuplas()
        {
            var numeros1 = (1, 2);
            WriteLine($"{numeros1.Item1}, {numeros1.Item2}");

            (int one, int two) numeros2 = (1, 2);
            WriteLine($"{numeros2.Item1}, {numeros2.Item2}");
            WriteLine($"{numeros2.one}, {numeros2.two}");

            var numeros3 = (one: 1, two: 2);
            WriteLine($"{numeros3.one}, {numeros3.two}");

            (int uno, int due) numeros4 = (one: 1, two: 2);
            WriteLine($"{numeros4.uno}, {numeros4.due}");
            //WriteLine($"{numeros4.one}, {numeros4.two}"); --> erro!
        }

        static void Desconstrucao()
        {
            // var (nome, _) = Pessoa.Get();

            var (nome, empresa) = Pessoa.Get();
            WriteLine($"{nome}, {empresa}");

            //tuplas
            var numeros = (one: 1, two: 2);
            var (um, dois) = numeros;
            WriteLine($"{um}, {dois}");
        }

        class Pessoa
        {
            public string Nome { get; set; }
            public string Empresa { get; set; }

            public void Deconstruct(out string nome, out string empresa)
            {
                nome = Nome;
                empresa = Empresa;
            }

            public static Pessoa Get() => new Pessoa { Nome = "...", Empresa = "..." };
        }

        static void OutVariables()
        {
            if (int.TryParse("1", out int result))
                WriteLine(result);

            if (int.TryParse("1", out var result2))
                WriteLine(result2);
        }

        static void Discards()
        {
            (int x, int y) BuscarCoordenadas() => (1, 2);

            var (_, y) = BuscarCoordenadas();

            if (int.TryParse("1", out _))
                WriteLine("É um número :)");

            object o = 1;
            if (o is Pessoa p)
                WriteLine(p.Empresa);
            else if (o is null)
                WriteLine("null");
            else if (o is var _)
                WriteLine("Desconhecido");

        }

        ref int Buscar(int[,] matriz, Func<int, bool> predicado)
        {
            for (int i = 0; i < matriz.GetLength(0); i++)
                for (int j = 0; j < matriz.GetLength(1); j++)
                    if (predicado(matriz[i, j]))
                        return ref matriz[i, j];

            throw new InvalidOperationException("não encontrado");
        }

        void RefLocals()
        {
            var matriz = new[,] { { 1, 2, 3, 4, 5 }, { 6, 7, 8, 42, 10 } };
            ref var item = ref Buscar(matriz, x => x == 42);
            WriteLine(item);
            item = 24;
            WriteLine(matriz[1, 3]);
        }

        public class ExpressionBodied
        {
            public ExpressionBodied(string nome) => Nome = nome;

            ~ExpressionBodied() => WriteLine("Finalizado!");

            private string nome;
            public string Nome
            {
                get => nome.ToLower();
                set => nome = value;
            }
        }

        private string a = GetA() ?? throw new Exception();

        private static string GetA() => throw new Exception();

        static void Numeric()
        {
            int dezesseis = 0b0001_0000;
            int trintaEDois = 0b010_0000;
            int sessentaEQuatro = 0b0100_0000;
            int centoEVinteOito = 0b1000_0000;

            long cemBilhoes = 100_000_000_000;

            double constanteDeAvogadro = 6.022_140_857_747_474e23;

            decimal proporcaoAurea = 1.618_033_988_749M;
        }

        double Media(IEnumerable<int> numeros)
        {
            double Dividir(double a, double b)
            {
                if (b == 0)
                    throw new DivideByZeroException();
                return a / b;
            }

            var soma = numeros.Sum();

            return Dividir(soma, numeros.Count());
        }

        void Adiciona1ESomaTudo(int[] numeros)
        {
            IEnumerable<int> Somar1(IEnumerable<int> ns)
            {
                foreach (var n in ns)
                    yield return n + 1;
            }

            if (numeros == null)
                throw new NullReferenceException();

            WriteLine(Somar1(numeros).Sum());
        }

        public async ValueTask<int> Get8()
        {
            Task.Delay(100);
            return 8;
        }

        public async ValueTask<int> SomaAsync(IEnumerable<ValueTask<int>> numeros)
        {
            var soma = 0;
            foreach (var nTask in numeros)
            {
                var n = await nTask;
                soma += n;
            }

            return soma;
        }

        void InferTupleNames()
        {
            var x = (f1: 1, f2: 2);

            var tupla = (x.f1, x.f2, x);
        }

        void DefaultLiteralExpressions()
        {
            Func<string, bool> predicate = default;
            int i = default;
            N(default);
        }

        (int, int) N(int[] ns = default) => default;

        string O()
        {
            return default;
        }

        T P<T>()
        {
            T t = default;
            return t;
        }

        void M(int i, int j) { };

        void N()
        {
            M(i: 2, 3);
        }

        void Underscore()
        {

            var m = 0b_101; //5

            var n = 0x_00C0; //192

        }

        class Base
        {
            private protected int f;
        }

        class DerivadaMesmoAssembly : Base
        {
            void M()
            {
                var x = base.f;
            }
        }

        class DerivadaOutroAssembly : Base
        {
            void M()
            {
                var x = base.f; //ERRO!
            }
        }

        class Ponto
        {
            public Ponto(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public int Y { get; set; }
        }

        Vector Add(in Vector v1, in Vector v2)
        {
            v1 = default(Vector); //não funciona!
            v1.X = 0; //não funciona
            Foo(ref v1.X); //não funciona!

            //OK:
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Ponto Soma(in this Ponto p1, in p2) => new Ponto(p1.X + p2.X, p1.Y + p2.Y);

        void In()
        {
            Ponto p1 = new Ponto(1, 2), p2 = new Ponto(3, 4);
            var pResultado = p1.Soma(p2);
            var pResultado2 = p1.Soma(new Ponto(3, 4));
        }

        void Metodo(string? stringQuePodeSerNula)
        {
            WriteLine(stringQuePodeSerNula.Length); //warning

            WriteLine(stringQuePodeSerNula!.Length); // no warning
        }

        interface I
        {
            void Metodo() { WriteLine("Interface Method"); }
        }

        class C : I { } //OK

        void M()
        {
            I i new C();
            i.Metodo(); // vai printar "Interface Method"
        }
    }
}
