using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conecta4Diagonal
{
    class Program
    {
        public static int [] Diagonal = new int[2];
        const int LINHA = 6;
        const int COLUNA = 7;

        static void Main(string[] args)
        {
            //Represantação do Tabuleiro para testes
            int[,] jogo = new int[LINHA, COLUNA]{
                { 2,0,0,2,0,2,0 },
                { 0,1,0,0,2,0,0 },
                { 0,0,2,1,0,1,1 },
                { 0,0,1,2,0,2,1 },
                { 0,2,0,0,2,0,1 },
                { 2,0,0,1,0,2,0 },
            };

            int Vencedor = -1;
            int[] jogada = new int[2];
            //Última jogada
            jogada[0] = 5;
            jogada[1] = 5;

            //testando os métodos
            Vencedor = VencedorDiagonalDireita(jogo, jogada);
            if (Vencedor != -1)
            {
                Console.WriteLine("Diagonal Superior Direita - Jogador " + Vencedor);
            }
            else
            {
                Vencedor = VencedorDiagonalEsquerda(jogo, jogada);
                if (Vencedor != -1)
                {
                    Console.WriteLine("Diagonal Superior Esquerda - Jogador " + Vencedor);
                }
                else
                {
                    Console.WriteLine("Continuar Jogando " + Vencedor);
                }
            }
            Console.ReadKey();
        }

        //Verifica na diagonal da de cima para baixo da direita para esquerda
        public static int VencedorDiagonalDireita(int[,] t, int[] j)
        {
            int contador = 0;
            int TamanhoDiagonal=0;
            int l = j[0]; //linha da última jogada
            int c = j[1]; //coluna da ultima jogada

            //Localiza onde inicia a diagonal
            if (l - c >= 0)
            {
                l = l - c; //linha
                c = 0; //coluna
                TamanhoDiagonal = LINHA - l; //tamanho da diagonal para usar no for()
            } else
            {
                c = c - l;
                l = 0;
                TamanhoDiagonal = COLUNA - c; //tamanho da diagonal para usar no for()
            }

            //Varre a diagonal da ultima jogada
            for (int v = 0; v < TamanhoDiagonal; v++) 
            {
                //Compara se os item presentes na diagonal são iguais a ultima jogada
                if (t[j[0], j[1]] == t[l + v, c + v])
                {
                    if (++contador == 4)
                    {
                        return t[j[0], j[1]];
                    }
                }
                else
                {
                    contador = 0;
                }
            }
            return -1;
        }

        //Verifica na diagonal da de cima para baixo da esquerda para direita
        public static int VencedorDiagonalEsquerda(int[,] t, int[] j)
        {
            int contador = 0;
            int TamanhoDiagonal = 0;
            int l = j[0];
            int c = j[1];

            //Localiza onde inicia a diagonal
            if (l + c < 6)
            {
                c = l + c; 
                l = 0;
                TamanhoDiagonal =  c + 1;
            }
            else
            {
                l = (l + c) - 6;
                c = 6;
                TamanhoDiagonal = c - l;
            }

            for (int v = 0; v < TamanhoDiagonal; v++)
            {
                if (t[j[0], j[1]] == t[l + v, c - v])
                {
                    if (++contador == 4)
                    {
                        return t[j[0], j[1]];
                    }
                }
                else
                {
                    contador = 0;
                }
            }
            return -1;
        }
    }
}

