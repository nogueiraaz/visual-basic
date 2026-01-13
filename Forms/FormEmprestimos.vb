Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data
Public Class FormEmprestimos
    Inherits Form
    
    Private dgvEmprestimos As DataGridView
    Private cmbUtilizador As ComboBox
    Private cmbLivro As ComboBox
    Private dtpDataEmprestimo As DateTimePicker
    Private btnRegistarEmprestimo As Button
    Private btnRegistarDevolucao As Button
    Private btnLimpar As Button
    Private lblId As Label
    Private idSelecionado As Integer = 0
    
    Public Sub New()
        InitializeComponent()
    End Sub
    
    Private Sub InitializeComponent()
        ' Configurar formulário
        Me.Text = "Gestão de Empréstimos"
        Me.Size = New Size(900, 650)
        Me.StartPosition = FormStartPosition.CenterScreen
        
        ' Label ID (oculto)
        lblId = New Label()
        lblId.Text = "ID: "
        lblId.Location = New Point(20, 20)
        lblId.Size = New Size(200, 25)
        Me.Controls.Add(lblId)
        
        ' Label Utilizador
        Dim lblUtilizador As New Label()
        lblUtilizador.Text = "Utilizador:"
        lblUtilizador.Location = New Point(20, 50)
        lblUtilizador.Size = New Size(80, 25)
        Me.Controls.Add(lblUtilizador)
        
        ' ComboBox Utilizador
        cmbUtilizador = New ComboBox()
        cmbUtilizador.Location = New Point(120, 50)
        cmbUtilizador.Size = New Size(250, 25)
        Me.Controls.Add(cmbUtilizador)
        
        ' Label Livro
        Dim lblLivro As New Label()
        lblLivro.Text = "Livro:"
        lblLivro.Location = New Point(390, 50)
        lblLivro.Size = New Size(60, 25)
        Me.Controls.Add(lblLivro)
        
        ' ComboBox Livro
        cmbLivro = New ComboBox()
        cmbLivro.Location = New Point(480, 50)
        cmbLivro.Size = New Size(350, 25)
        Me.Controls.Add(cmbLivro)
        
        ' Label Data Empréstimo
        Dim lblData As New Label()
        lblData.Text = "Data:"
        lblData.Location = New Point(20, 90)
        lblData.Size = New Size(60, 25)
        Me.Controls.Add(lblData)
        
        ' DateTimePicker
        dtpDataEmprestimo = New DateTimePicker()
        dtpDataEmprestimo.Location = New Point(120, 90)
        dtpDataEmprestimo.Size = New Size(200, 25)
        dtpDataEmprestimo.Value = Date.Now
        Me.Controls.Add(dtpDataEmprestimo)
        
        ' Botão Registar Empréstimo
        btnRegistarEmprestimo = New Button()
        btnRegistarEmprestimo.Text = "Registar Empréstimo"
        btnRegistarEmprestimo.Location = New Point(20, 130)
        btnRegistarEmprestimo.Size = New Size(150, 30)
        AddHandler btnRegistarEmprestimo.Click, AddressOf BtnRegistarEmprestimo_Click
        Me.Controls.Add(btnRegistarEmprestimo)
        
        ' Botão Registar Devolução
        btnRegistarDevolucao = New Button()
        btnRegistarDevolucao.Text = "Registar Devolução"
        btnRegistarDevolucao.Location = New Point(180, 130)
        btnRegistarDevolucao.Size = New Size(150, 30)
        AddHandler btnRegistarDevolucao.Click, AddressOf BtnRegistarDevolucao_Click
        Me.Controls.Add(btnRegistarDevolucao)
        
        ' Botão Limpar
        btnLimpar = New Button()
        btnLimpar.Text = "Limpar"
        btnLimpar.Location = New Point(340, 130)
        btnLimpar.Size = New Size(100, 30)
        AddHandler btnLimpar.Click, AddressOf BtnLimpar_Click
        Me.Controls.Add(btnLimpar)
        
        ' DataGridView
        dgvEmprestimos = New DataGridView()
        dgvEmprestimos.Location = New Point(20, 180)
        dgvEmprestimos.Size = New Size(850, 400)
        dgvEmprestimos.AllowUserToAddRows = False
        AddHandler dgvEmprestimos.CellClick, AddressOf DgvEmprestimos_CellClick
        Me.Controls.Add(dgvEmprestimos)
    End Sub
    
    Private Sub FormEmprestimos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CarregarUtilizadores()
        CarregarLivrosDisponiveis()
        CarregarEmprestimos()
    End Sub
    
    Private Sub CarregarUtilizadores()
        Try
            cmbUtilizador.Items.Clear()
            Dim utilizadores = UtilizadorDAL.Listar()
            
            For Each util In utilizadores
                cmbUtilizador.Items.Add(util.Id & " - " & util.Nome)
            Next
        Catch ex As Exception
            MessageBox.Show("Erro ao carregar utilizadores: " & ex.Message)
        End Try
    End Sub
    
    Private Sub CarregarLivrosDisponiveis()
        Try
            cmbLivro.Items.Clear()
            Dim livros = LivroDAL.Listar()
            
            For Each livro In livros
                If livro.Estado = "Disponível" Then
                    cmbLivro.Items.Add(livro.Id & " - " & livro.Titulo & " (" & livro.Autor & ")")
                End If
            Next
        Catch ex As Exception
            MessageBox.Show("Erro ao carregar livros: " & ex.Message)
        End Try
    End Sub
    
    Private Sub CarregarEmprestimos()
        Try
            dgvEmprestimos.DataSource = Nothing
            Dim emprestimos = EmprestimoDAL.Listar()
            
            If emprestimos.Count > 0 Then
                Dim dt As New DataTable()
                dt.Columns.Add("ID")
                dt.Columns.Add("ID Livro")
                dt.Columns.Add("ID Utilizador")
                dt.Columns.Add("Data Empréstimo")
                dt.Columns.Add("Data Devolução")
                dt.Columns.Add("Status")
                
                For Each emp In emprestimos
                    Dim dataDev As String = If(emp.DataDevolucao = Nothing, "", emp.DataDevolucao.ToString("dd/MM/yyyy"))
                    dt.Rows.Add(emp.Id, emp.IdLivro, emp.IdUtilizador, emp.DataEmprestimo.ToString("dd/MM/yyyy"), dataDev, emp.Status)
                Next
                
                dgvEmprestimos.DataSource = dt
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao carregar empréstimos: " & ex.Message)
        End Try
    End Sub
    
    Private Sub BtnRegistarEmprestimo_Click(sender As Object, e As EventArgs)
        If cmbUtilizador.SelectedIndex = -1 Then
            MessageBox.Show("Selecione um utilizador", "Aviso")
            Return
        End If
        
        If cmbLivro.SelectedIndex = -1 Then
            MessageBox.Show("Selecione um livro", "Aviso")
            Return
        End If
        
        ' Extrair IDs
        Dim idUtil = CInt(cmbUtilizador.SelectedItem.ToString().Split("-")(0).Trim())
        Dim idLivro = CInt(cmbLivro.SelectedItem.ToString().Split("-")(0).Trim())
        
        ' Criar empréstimo
        Dim emprestimo As New Emprestimo()
        emprestimo.IdUtilizador = idUtil
        emprestimo.IdLivro = idLivro
        emprestimo.DataEmprestimo = dtpDataEmprestimo.Value.Date
        emprestimo.Status = "Ativo"
        
        If EmprestimoDAL.Inserir(emprestimo) Then
            MessageBox.Show("Empréstimo registado com sucesso!", "Sucesso")
            BtnLimpar_Click(Nothing, Nothing)
            CarregarEmprestimos()
            CarregarLivrosDisponiveis()
        Else
            MessageBox.Show("Erro ao registar empréstimo", "Erro")
        End If
    End Sub
    
    Private Sub BtnRegistarDevolucao_Click(sender As Object, e As EventArgs)
        If idSelecionado = 0 Then
            MessageBox.Show("Selecione um empréstimo para registar a devolução", "Aviso")
            Return
        End If
        
        If MessageBox.Show("Tem certeza que deseja registar a devolução deste livro?", "Confirmação", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            If EmprestimoDAL.RegistarDevolucao(idSelecionado) Then
                MessageBox.Show("Devolução registada com sucesso!", "Sucesso")
                BtnLimpar_Click(Nothing, Nothing)
                CarregarEmprestimos()
                CarregarLivrosDisponiveis()
            Else
                MessageBox.Show("Erro ao registar devolução", "Erro")
            End If
        End If
    End Sub
    
    Private Sub BtnLimpar_Click(sender As Object, e As EventArgs)
        cmbUtilizador.SelectedIndex = -1
        cmbLivro.SelectedIndex = -1
        dtpDataEmprestimo.Value = Date.Now
        lblId.Text = "ID: "
        idSelecionado = 0
    End Sub
    
    Private Sub DgvEmprestimos_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row = dgvEmprestimos.Rows(e.RowIndex)
            idSelecionado = CInt(row.Cells("ID").Value)
            lblId.Text = "ID: " & idSelecionado.ToString()
        End If
    End Sub
End Class