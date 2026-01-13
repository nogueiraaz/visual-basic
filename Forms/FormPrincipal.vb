Imports System.Windows.Forms
Imports System.Drawing

Public Class FormLogin
    Inherits Form
    
    Private txtEmail As TextBox
    Private txtPassword As TextBox
    Private btnEntrar As Button
    Private lblMensagem As Label
    Public UtilizadorLogado As Utilizador = Nothing
    Public TipoUtilizador As String = ""
    
    Public Sub New()
        InitializeComponent()
    End Sub
    
    Private Sub InitializeComponent()
        Me.Text = "Login - Biblioteca Escolar"
        Me.Size = New Size(400, 300)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        
        ' Label Título
        Dim lblTitulo As New Label()
        lblTitulo.Text = "Sistema de Gestão de Biblioteca"
        lblTitulo.Font = New Font("Arial", 14, FontStyle.Bold)
        lblTitulo.Location = New Point(50, 20)
        lblTitulo.Size = New Size(300, 30)
        lblTitulo.TextAlign = ContentAlignment.MiddleCenter
        Me.Controls.Add(lblTitulo)
        
        ' Label Email/Utilizador
        Dim lblEmail As New Label()
        lblEmail.Text = "Utilizador (ID ou Nome):"
        lblEmail.Location = New Point(30, 70)
        lblEmail.Size = New Size(150, 25)
        Me.Controls.Add(lblEmail)
        
        ' TextBox Email
        txtEmail = New TextBox()
        txtEmail.Location = New Point(30, 95)
        txtEmail.Size = New Size(340, 25)
        Me.Controls.Add(txtEmail)
        
        ' Label Password
        Dim lblPassword As New Label()
        lblPassword.Text = "Contacto (Password):"
        lblPassword.Location = New Point(30, 130)
        lblPassword.Size = New Size(150, 25)
        Me.Controls.Add(lblPassword)
        
        ' TextBox Password
        txtPassword = New TextBox()
        txtPassword.Location = New Point(30, 155)
        txtPassword.Size = New Size(340, 25)
        txtPassword.UseSystemPasswordChar = True
        Me.Controls.Add(txtPassword)
        
        ' Botão Entrar
        btnEntrar = New Button()
        btnEntrar.Text = "Entrar"
        btnEntrar.Location = New Point(150, 200)
        btnEntrar.Size = New Size(100, 35)
        AddHandler btnEntrar.Click, AddressOf BtnEntrar_Click
        Me.Controls.Add(btnEntrar)
        
        ' Label Mensagem
        lblMensagem = New Label()
        lblMensagem.Text = ""
        lblMensagem.ForeColor = Color.Red
        lblMensagem.Location = New Point(30, 240)
        lblMensagem.Size = New Size(340, 25)
        lblMensagem.TextAlign = ContentAlignment.MiddleCenter
        Me.Controls.Add(lblMensagem)
    End Sub
    
    Private Sub BtnEntrar_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtEmail.Text) Then
            lblMensagem.Text = "Introduza um utilizador"
            Return
        End If
        
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            lblMensagem.Text = "Introduza o contacto"
            Return
        End If
        
        Try
            ' Procurar utilizador por ID ou Nome
            Dim utilizadores = UtilizadorDAL.Li
star()
Dim utilizadorEncontrado As Utilizador = Nothing

        For Each util In utilizadores
            If util.Id.ToString() = txtEmail.Text OrElse util.Nome.ToLower() = txtEmail.Text.ToLower() Then
                ' Verificar se o contacto corresponde (simples autenticação)
                If util.Contacto = txtPassword.Text Then
                    utilizadorEncontrado = util
                    Exit For
                End If
            End If
        Next
        
        If utilizadorEncontrado IsNot Nothing Then
            ' Login bem-sucedido
            UtilizadorLogado = utilizadorEncontrado
            
            ' Obter tipo de utilizador (Admin/User)
            ' Por enquanto, todos são User (pode ser expandido)
            TipoUtilizador = "User"
            
            ' Fechar formulário de login
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            lblMensagem.Text = "Utilizador ou contacto inválido"
            txtPassword.Clear()
        End If
    Catch ex As Exception
        lblMensagem.Text = "Erro ao autenticar: " & ex.Message
    End Try
End Sub
End Class


---

## **PASSO 3: Modificar FormPrincipal.vb** (com histórico)

```vb
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data

Public Class FormPrincipal
    Inherits Form
    
    Private dgvHistorico As DataGridView
    Private lblBemvindo As Label
    Private utilizadorLogado As Utilizador
    Private tipoUtilizador As String
    
    Public Sub New(util As Utilizador, tipo As String)
        InitializeComponent()
        utilizadorLogado = util
        tipoUtilizador = tipo
    End Sub
    
    Private Sub InitializeComponent()
        Me.Text = "Sistema de Gestão de Biblioteca Escolar"
        Me.Size = New Size(1200, 700)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.WindowState = FormWindowState.Maximized
        
        ' Label Bem-vindo
        lblBemvindo = New Label()
        lblBemvindo.Text = "Bem-vindo, " & utilizadorLogado.Nome & "!"
        lblBemvindo.Font = New Font("Arial", 14, FontStyle.Bold)
        lblBemvindo.Location = New Point(20, 20)
        lblBemvindo.Size = New Size(400, 30)
        Me.Controls.Add(lblBemvindo)
        
        ' DataGridView para histórico
        dgvHistorico = New DataGridView()
        dgvHistorico.Location = New Point(20, 70)
        dgvHistorico.Size = New Size(Me.ClientSize.Width - 40, Me.ClientSize.Height - 150)
        dgvHistorico.AllowUserToAddRows = False
        dgvHistorico.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Me.Controls.Add(dgvHistorico)
    End Sub
    
    Private Sub FormPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CriarMenu()
        CarregarHistoricoEmprestimos()
    End Sub
    
    Private Sub CriarMenu()
        Dim menuStrip As New MenuStrip()
        
        ' Menu Ficheiro
        Dim menuFicheiro As New ToolStripMenuItem("Ficheiro")
        menuFicheiro.DropDownItems.Add("Sair", Nothing, Sub() Me.Close())
        
        ' Menu Gestão (apenas para Admin)
        If tipoUtilizador = "Admin" Then
            Dim menuGestao As New ToolStripMenuItem("Gestão")
            menuGestao.DropDownItems.Add("Utilizadores", Nothing, AddressOf AbrirFormUtilizadores)
            menuGestao.DropDownItems.Add("Livros", Nothing, AddressOf AbrirFormLivros)
            menuGestao.DropDownItems.Add("Empréstimos", Nothing, AddressOf AbrirFormEmprestimos)
            menuStrip.Items.Add(menuGestao)
        Else
            ' Menu simplificado para Users
            Dim menuAcoes As New ToolStripMenuItem("Minhas Ações")
            menuAcoes.DropDownItems.Add("Ver Empréstimos", Nothing, AddressOf AbrirFormEmprestimos)
            menuStrip.Items.Add(menuAcoes)
        End If
        
        ' Menu Ajuda
        Dim menuAjuda As New ToolStripMenuItem("Ajuda")
        menuAjuda.DropDownItems.Add("Sobre", Nothing, AddressOf MenuSobre)
        
        menuStrip.Items.Add(menuFicheiro)
        menuStrip.Items.Add(menuAjuda)
        
        Me.MainMenuStrip = menuStrip
        Me.Controls.Add(menuStrip)
    End Sub
    
    Private Sub CarregarHistoricoEmprestimos()
        Try
            dgvHistorico.DataSource = Nothing
            Dim emprestimos = EmprestimoDAL.ListarTodos()
            
            If emprestimos.Count > 0 Then
                Dim dt As New DataTable()
                dt.Columns.Add("ID Empréstimo")
                dt.Columns.Add("Utilizador")
                dt.Columns.Add("Livro")
                dt.Columns.Add("Data Empréstimo")
                dt.Columns.Add("Data Devolução")
                dt.Columns.Add("Status")
                
                For Each emp In emprestimos
                    ' Obter dados do utilizador e livro
                    Dim utilList = UtilizadorDAL.Listar()
                    Dim livList = LivroDAL.Listar()
                    
                    Dim nomeUtil = utilList.FirstOrDefault(Function(u) u.Id = emp.IdUtilizador)?.Nome ?? "Desconhecido"
                    Dim nomeLiv = livList.FirstOrDefault(Function(l) l.Id = emp.IdLivro)?.Titulo ?? "Desconhecido"
                    
                    Dim dataDev As String = If(emp.DataDevolucao = Nothing, "-", emp.DataDevolucao.ToString("dd/MM/yyyy"))
                    
                    dt.Rows.Add(emp.Id, nomeUtil, nomeLiv, emp.DataEmprestimo.ToString("dd/MM/yyyy"), dataDev, emp.Status)
                Next
                
                dgvHistorico.DataSource = dt
            Else
                MessageBox.Show("Nenhum empréstimo registado", "Informação")
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao carregar histórico: " & ex.Message)
        End Try
    End Sub
    
    Private Sub AbrirFormUtilizadores()
        Try
            Dim form As New FormUtilizadores()
            form.ShowDialog()
            CarregarHistoricoEmprestimos()
        Catch ex As Exception
            MessageBox.Show("Erro: " & ex.Message)
        End Try
    End Sub
    
    Private Sub AbrirFormLivros()
        Try
            Dim form As New FormLivros()
            form.ShowDialog()
            CarregarHistoricoEmprestimos()
        Catch ex As Exception
            MessageBox.Show("Erro: " & ex.Message)
        End Try
    End Sub
    
    Private Sub AbrirFormEmprestimos()
        Try
            Dim form As New FormEmprestimos()
            form.ShowDialog()
            CarregarHistoricoEmprestimos()
        Catch ex As Exception
            MessageBox.Show("Erro: " & ex.Message)
        End Try
    End Sub
    
    Private Sub MenuSobre()
        MessageBox.Show("Sistema de Gestão de Biblioteca Escolar v1.0" & vbCrLf & 
                        "Desenvolvido em VB.NET com MySQL", "Sobre")
    End Sub
End Class