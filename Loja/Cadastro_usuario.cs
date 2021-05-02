using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Loja.DTO;
using Loja.BLL;

namespace Loja
{
    public partial class Cadastro_usuario : Form
    {
        string modo = "";
        int codUsuSelecionado = -1;

        public Cadastro_usuario()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtCadastro_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*Linha atual que estiver selecionada aparecerá nos campos (textbox) acima do dataGrid*/

            int sel = dataGridView1.CurrentRow.Index;
            /*Valor de cada datagrid será enviado ao seu respectivo texbos*/
            txtNome.Text = Convert.ToString(dataGridView1["nome", sel].Value);
            txtLogin.Text = Convert.ToString(dataGridView1["login", sel].Value);
            txtEmail.Text = Convert.ToString(dataGridView1["email", sel].Value);
            txtSenha.Text = Convert.ToString(dataGridView1["senha", sel].Value);
            txtCadastro.Text = Convert.ToString(dataGridView1["cadastro", sel].Value);
            codUsuSelecionado = Convert.ToInt32(dataGridView1["cod_usuario", sel].Value);

            /*Condição se a situação for igual a "A" então combobox ficará Ativo senão "Inativo*/

            if (Convert.ToString(dataGridView1["situacao", sel].Value) == "A")
            {
                cboStituacao.Text = "Ativo";
            }
            else
            {
                cboStituacao.Text = "Inativo";               
            }
             switch (Convert.ToString(dataGridView1["perfil", sel].Value))
            {
                /*Caso seja 1, será escolhido Administrados, caso seja 2, operador e caso 3, Gerencial*/

                case "1":
                    cboPerfil.Text = "Administrador";
                    break;
                case "2":
                    cboPerfil.Text = "Operador";
                    break;
                case "3":
                    cboPerfil.Text = "Gerencial";
                    break;
            }

        }

        private void Cadastro_usuario_Load(object sender, EventArgs e)
        /*Método carregaGrid.
         *Para atualizar os dados do Grid.
         *Basta chamar o Método.*/
        {
            carregaGrid();
            lblMensagem.Text = "";
        }

        private void carregaGrid()
        {
            try
            {
                IList<usuario_DTO> listUsuario_DTO = new List<usuario_DTO>();
                listUsuario_DTO = new UsuarioBLL().cargaUsuario();

                /*Preencher dados no DataGridView*/
                dataGridView1.DataSource = listUsuario_DTO;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            //Chamando método Limpar Campos que foi criando
            limpar_campos();

            /*inserindo data atual automaticamente no txtCadastro, ao clicar no botão novo*/
            txtCadastro.Text = Convert.ToString(System.DateTime.Now);

            /*após clicar no botão, modo passa a ser novo (incluindo um registro)*/
            modo = "novo";
        }

        /*Criando Método Limpar Campos,´para que todas as vezes em 
         *que for necessário limpar não será necessário repetir o 
         *código, apenas chamar o método*/

         private void limpar_campos()
        {
            txtNome.Text = "";
            txtLogin.Text = "";
            txtEmail.Text = "";
            txtCadastro.Text = "";
            cboPerfil.Text = "";
            cboStituacao.Text = "";
            txtSenha.Text = "";
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if ( codUsuSelecionado < 0)
            {
                lblMensagem.Text = "Selecione um usuario antes de prosseguir";
                return;
            }
            modo = "altera";
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if (codUsuSelecionado < 0)
            {
                lblMensagem.Text = "Selecione um usuario antes";
                return;
            }
            else
            {
                lblMensagem.Text = "";
                modo = "excluir";
                MessageBox.Show("Para excluir o usuario, clique em confirmar");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            modo = "";
            limpar_campos();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (modo == "novo")
            {
                /*Tratamento de Erros, exibe msg*/
                try
                {

                    /*Objetivo USU*/

                    usuario_DTO USU = new usuario_DTO();
                    USU.nome = txtNome.Text;
                    USU.login = txtLogin.Text;
                    USU.email = txtEmail.Text;
                    USU.cadastro = System.DateTime.Now;
                    USU.senha = txtSenha.Text;
                    

                    if (cboStituacao.Text == "Ativo")
                    {
                        USU.situacao = "A";
                    }
                    else
                    {
                        USU.situacao = "I";
                    }
                    switch (cboPerfil.Text)
                    {
                        case "Administrador":
                            USU.perfil = 1;
                            break;
                        case "Operador":
                            USU.perfil = 2;
                            break;
                        case "Gerencial":
                            USU.perfil = 3;
                            break;
                    }


                    /*Método insere usuáiro na classe UsuarioBLL*/

                    int x = new UsuarioBLL().insereUsuario(USU);
                    /*Verifica se houve alguma gravação*/
                    if (x > 0)
                    {
                        MessageBox.Show("Gravado com Sucesso!");
                    }

                    /*Recarrega o Grid após os dados erem gravados*/
                    carregaGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro inesperado"  + ex.Message);
                }
            }
            if (modo == "altera")
            {
                /*Tratamento de Erros, exibe msg*/
                try
                {
                    if (codUsuSelecionado < 0)
                    {
                        lblMensagem.Text = "Selecione um usuario antes de prosseguir";
                        return;
                    }

                    /*Objeto USU, assim como feit no modo="novo" Lê os textbox com os dados alterados*/

                    usuario_DTO USU = new usuario_DTO();
                    USU.cod_usuario = codUsuSelecionado;
                    USU.nome = txtNome.Text;
                    USU.login = txtLogin.Text;
                    USU.email = txtEmail.Text;
                    USU.cadastro = System.DateTime.Now;
                    USU.senha = txtSenha.Text;
                    
                    if (cboStituacao.Text == "Ativo")
                    {
                        USU.situacao = "A";
                    }
                    else
                    {
                        USU.situacao = "I";
                    }
                    switch (cboPerfil.Text)
                    {
                        case "Administrados":
                            USU.perfil = 1;
                            break;
                        case "Operador":
                            USU.perfil = 2;
                            break;
                        case "Gerencial":
                            USU.perfil = 3;
                            break;
                    }

                    int x = new UsuarioBLL().alteraUsuario(USU);
                    /*Verifica se houve alguma gravação*/
                    if (x > 0)
                    {
                        MessageBox.Show("Alterado com Sucesso!");
                    }

                    /*Recarrrega o Grid após os dados serem gravados*/
                    carregaGrid();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro inesperado" + ex.Message);
                }
            }
            if (modo == "excluir")
            {
                try
                {
                    if (codUsuSelecionado < 0)
                    {
                        lblMensagem.Text = "Selecione um usuario antes de processiguir";
                        return;
                    }
                    usuario_DTO USU = new usuario_DTO();
                    USU.cod_usuario = codUsuSelecionado;                    
                    int x = new UsuarioBLL().deletaUsuario(USU);
                    if (x > 0)
                    {
                        MessageBox.Show("Excluido com Sucesso!");
                    }

                    /*Recarrega o Grid após os dados serem gravados*/
                    carregaGrid();
                    limpar_campos();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro inesperado" + ex.Message);
                }
            }
            modo = "";
        }

        private void txtSenha_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
