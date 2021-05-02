using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loja.DTO;

namespace Loja.DAL
{
   public class UsuarioDAL
    {
        /* Metodo cargaUsuario, retorna uma Lista de objetos usuario DTO
         *(composto por vários atributos), vai até o BD e busca todos os usuários
         *Usamos o try e Catch caso dê algum erro, retorna para a camada view
         *Executar o método cargaUsuario (será criado na DAL)*/

        public IList<usuario_DTO> cargaUsuario()
        {
            try
            {
                /*Conexão com BD*Selecione todos os dados a tb_usuarios*/
                SqlConnection CON = new SqlConnection();
                CON.ConnectionString = Properties.Settings.Default.CST;
                SqlCommand CM = new SqlCommand();
                CM.CommandType = System.Data.CommandType.Text;
                CM.CommandText = "SELECT*FROM tb_usuarios";
                CM.Connection = CON;
                

                SqlDataReader ER;
                IList<usuario_DTO> listUsuarioDTO = new List<usuario_DTO>();

                CON.Open();
                ER = CM.ExecuteReader();
                if (ER.HasRows)
                {
                    while (ER.Read())
                    {
                        usuario_DTO usu = new usuario_DTO();
                        

                            /* nome dos objetos criados na DTO *Cada objeto criado é enviado para a list, posibilitando
                             * que no finl vc tenha uma lita com vários usuários*/

                        usu.cod_usuario = Convert.ToInt32(ER["cod_usuario"]);
                        usu.perfil = Convert.ToInt32(ER["perfil"]);
                        usu.cadastro = Convert.ToDateTime(ER["cadastro"]);
                        usu.nome = Convert.ToString(ER["nome"]);
                        usu.email = Convert.ToString(ER["email"]);
                        usu.login = Convert.ToString(ER["cadastro"]);
                        usu.senha = Convert.ToString(ER["senha"]);
                        usu.situacao = Convert.ToString(ER["situacao"]);                        
                        listUsuarioDTO.Add(usu);

                       
                    }
                }
                return listUsuarioDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int insereUsuario (usuario_DTO USU)
        {
            try
            {
                /*Conexão com BD * Inserindo dados na tb_usuarios*/

                SqlConnection CON = new SqlConnection();
                CON.ConnectionString = Properties.Settings.Default.CST;
                SqlCommand CM = new SqlCommand();
                CM.CommandType = System.Data.CommandType.Text;
                CM.CommandText = "INSERT INTO tb_usuarios (nome, login, email, senha, cadastro, situacao, perfil)" +
                "VALUES (@nome, @login, @email, @senha, @cadastro, @situacao, @perfil)";
                

                /*Parameters irá substituir os valores dentro do campo*/
                CM.Parameters.Add("nome", System.Data.SqlDbType.VarChar).Value = USU.nome;
                CM.Parameters.Add("login", System.Data.SqlDbType.VarChar).Value = USU.login;
                CM.Parameters.Add("email", System.Data.SqlDbType.VarChar).Value = USU.email;
                CM.Parameters.Add("senha", System.Data.SqlDbType.VarChar).Value = USU.senha;
                CM.Parameters.Add("cadastro", System.Data.SqlDbType.DateTime).Value = USU.cadastro;
                CM.Parameters.Add("situacao", System.Data.SqlDbType.NVarChar).Value = USU.situacao;
                CM.Parameters.Add("perfil", System.Data.SqlDbType.Int).Value = USU.perfil;

                CM.Connection = CON;

                CON.Open();
                int qtd = CM.ExecuteNonQuery();
                return qtd;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int alteraUsuario(usuario_DTO USU)
        {
            try
            {
                SqlConnection CON = new SqlConnection();
                CON.ConnectionString = Properties.Settings.Default.CST;
                SqlCommand CM = new SqlCommand();
                CM.CommandType = System.Data.CommandType.Text;

                /*Atenção ao nome dos campos que deve igual ao Banco de Dados*/
                CM.CommandText = "UPTADE tb_usuarios SET perfil=@perfil," +
                 "nome=@nome," + "login=@login," + "email=@email," + "senha=@senha," +
                 "cadastro=@cadastro," + "situacao=@situacao," + "WHERE cod_usuario=@cod_usuario";
                

                /*Parameters ira substituir os valores dentro do campo*/
                CM.Parameters.Add("perfil", System.Data.SqlDbType.Int).Value = USU.perfil;
                CM.Parameters.Add("nome", System.Data.SqlDbType.VarChar).Value = USU.nome;
                CM.Parameters.Add("login", System.Data.SqlDbType.VarChar).Value = USU.login;
                CM.Parameters.Add("email", System.Data.SqlDbType.VarChar).Value = USU.email;
                CM.Parameters.Add("senha", System.Data.SqlDbType.VarChar).Value = USU.senha;
                CM.Parameters.Add("cadastro", System.Data.SqlDbType.DateTime).Value = USU.cadastro;
                CM.Parameters.Add("situacao", System.Data.SqlDbType.VarChar).Value = USU.situacao;
                CM.Parameters.Add("cod_usuario", System.Data.SqlDbType.VarChar).Value = USU.cod_usuario;
                CM.Connection = CON;

                /*Abre conexão*/

                CON.Open();
                int qtd = CM.ExecuteNonQuery();
                return qtd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int deletaUsuario(usuario_DTO USU)
        {
            try
            {
                /*Excluindo dados na tb_usuarios*/

                SqlConnection CON = new SqlConnection();
                CON.ConnectionString = Properties.Settings.Default.CST;
                SqlCommand CM = new SqlCommand();
                CM.CommandType = System.Data.CommandType.Text;
                CM.CommandText = "DELETE tb_usuarios WHERE cod_usuario = @cod_usuario";
                

                /*Tem um unico parametro que sera o código do usuário, só exite um*/
                CM.Parameters.Add("cod_usuario", System.Data.SqlDbType.Int).Value = USU.cod_usuario;

                CM.Connection = CON;

                CON.Open();
                int qtd = CM.ExecuteNonQuery();

                /*Retorna registros afetados*/

                return qtd;    
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
