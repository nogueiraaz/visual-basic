Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data

Public Class FormPrincipal
    Inherits Form
    
    Private dgvHistorico As DataGridView
    Private lblBemvindo As Label
    Private lblTituloHistorico As Label
    Private utilizadorLogado As Utilizador
    Private tipoUtilizador As String
    
    Public Sub New(util As Utilizador, tipo As String)
        utilizadorLogado = util
        tipoUtilizador = tipo
        InitializeComponent()
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
        lblBemvindo.Location = New Point(20, 30)
        lblBemvindo.Size = New Size(600, 25)
        Me.Controls.Add(lblBemvindo)
        
        ' Label Título do Histórico
        lblTituloHistorico = New Label()
        lblTituloHistorico.Text = "Histórico de Empréstimos"
        lblTituloHistorico.Font = New Font("Arial", 12, FontStyle.Bold)
        lblTituloHistorico.Location = New Point(20, 70)
        lblTituloHistorico.Size = New Size(300, 25)
        Me.Controls.Add(lblTituloHistorico)
        
        ' DataGridView para histórico - OCUPA TODA A LARGURA
        dgvHistorico = New DataGridView()
        dgvHistorico.Location = New Point(20, 100)
        dgvHistorico.Size = New Size(Me.ClientSize.Width - 40, 300)
        dgvHistorico.AllowUserToAddRows = False
        dgvHistorico.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvHistorico.ReadOnly = True
        
        ' Estilo do Grid - INVISÍVEL
        dgvHistorico.GridColor = Color.White
        dgvHistorico.BackgroundColor = Color.White
        dgvHistorico.BorderStyle = BorderStyle.None
        dgvHistorico.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
        dgvHistorico.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvHistorico.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10, FontStyle.Bold)
        dgvHistorico.DefaultCellStyle.BackColor = Color.White
        dgvHistorico.DefaultCellStyle.ForeColor = Color.Black
        dgvHistorico.DefaultCellStyle.SelectionBackColor = Color.LightBlue
        dgvHistorico.DefaultCellStyle.SelectionForeColor = Color.Black
        
        Me.Controls.Add(dgvHistorico)
    End Sub
    
    Private Sub FormPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CriarMenu()
        CarregarHistoricoEmprestimos()
    End Sub
    
    Private Sub CriarMenu()
        Dim menuStrip As New MenuStrip()
        
        Dim menuFicheiro As New ToolStripMenuItem("Ficheiro")
        menuFicheiro.DropDownItems.Add("Sair", Nothing, Sub() Me.Close())
        
        If tipoUtilizador = "Admin" Then
            Dim menuGestao As New ToolStripMenuItem("Gestão")
            menuGestao.DropDownItems.Add("Utilizadores", Nothing, AddressOf AbrirFormUtilizadores)
            menuGestao.DropDownItems.Add("Livros", Nothing, AddressOf AbrirFormLivros)
            menuGestao.DropDownItems.Add("Empréstimos", Nothing, AddressOf AbrirFormEmprestimos)
            menuStrip.Items.Add(menuGestao)
        Else
            Dim menuAcoes As New ToolStripMenuItem("Minhas Ações")
            menuAcoes.DropDownItems.Add("Ver Empréstimos", Nothing, AddressOf AbrirFormEmprestimos)
            menuStrip.Items.Add(menuAcoes)
        End If
        
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
            
            ' Se for User, mostrar apenas seus empréstimos
            If tipoUtilizador <> "Admin" Then
                Dim emprestimosUtilizador As New List(Of Emprestimo)
                For Each emp In emprestimos
                    If emp.IdUtilizador = utilizadorLogado.Id Then
                        emprestimosUtilizador.Add(emp)
                    End If
                Next
                emprestimos = emprestimosUtilizador
            End If
            
            If emprestimos.Count > 0 Then
                Dim dt As New DataTable()
                dt.Columns.Add("ID")
                dt.Columns.Add("Utilizador")
                dt.Columns.Add("Livro")
                dt.Columns.Add("Data Empréstimo")
                dt.Columns.Add("Data Devolução")
                dt.Columns.Add("Status")
                
                Dim utilList = UtilizadorDAL.Listar()
                Dim livList = LivroDAL.Listar()
                
                For Each emp In emprestimos
                    Dim nomeUtil As String = "Desconhecido"
                    For Each u In utilList
                        If u.Id = emp.IdUtilizador Then
                            nomeUtil = u.Nome
                            Exit For
                        End If
                    Next
                    
                    Dim nomeLiv As String = "Desconhecido"
                    For Each l In livList
                        If l.Id = emp.IdLivro Then
                            nomeLiv = l.Titulo
                            Exit For
                        End If
                    Next
                    
                    Dim dataDev As String = ""
                    If emp.DataDevolucao = Nothing Then
                        dataDev = "-"
                    Else
                        dataDev = emp.DataDevolucao.ToString("dd/MM/yyyy")
                    End If
                    
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
        MessageBox.Show("Sistema de Gestão de Biblioteca Escolar v1.0" & vbCrLf & "Desenvolvido em VB.NET com MySQL", "Sobre")
    End Sub
End Class