using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;

namespace didaticos.redimensionador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando redimensionador");

            Thread thread = new Thread(Redimensionar);
            thread.Start();



            Console.Read();
        }

        static void Redimensionar()
        {
            #region "Diretorio"
            string diretorio_entrada = "Arquivo_Entrada";
            string diretorio_finalizado = "Arquivo_Finalizado";
            string diretorio_redimensionado = "Arquivo_Redimensionado";

            if (!Directory.Exists(diretorio_entrada))
            {
                Directory.CreateDirectory(diretorio_entrada);
            }

            if (!Directory.Exists(diretorio_finalizado))
            {
                Directory.CreateDirectory(diretorio_finalizado);
            }

            if (!Directory.Exists(diretorio_redimensionado))
            {
                Directory.CreateDirectory(diretorio_redimensionado);
            }

            #endregion

            FileStream fileStream;
            FileInfo fileInfo;

            while (true)
            {

                //Meu programa olha pasta de entrada
                //SE tiver arquivo ele ira redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorio_entrada);

                //ler tamanho redimensionar
                int novaAltura = 200;

                foreach (var arquivo in arquivosEntrada)
                {
                     fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                     fileInfo = new FileInfo(arquivo);

                    string caminho = Environment.CurrentDirectory + @"\" + diretorio_redimensionado 
                        + @"\"  + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name;

                    //redimensiona + //copia arquivos redimensionados para pasta Redimensionados
                    Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);

                    //fecha arquivo
                    fileStream.Close();

                    //move arquivo entrada para pasta finalizado
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizado 
                        + @"\" + fileInfo.Name;
                    
                    fileInfo.MoveTo(caminhoFinalizado);

                   
                }

                Thread.Sleep(new TimeSpan(0, 0, 3));

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem">Imagem a ser redimensionada</param>
        /// <param name="altura">Altura redimensionar</param>
        /// <param name="caminho">Caminho gravar arquivo redimensionado</param>
        /// <returns></returns>

        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);
            using (Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }

            novaImage.Save(caminho);
            imagem.Dispose();
        }
    }
}