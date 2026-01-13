Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data
Public Class FormLivros
    Inherits Form
    
    Private dgvLivros As DataGridView
    Private txtTitulo As TextBox
    Private txtAutor As TextBox
    Private txtAno As TextBox
    Private cmbEstado As ComboBox
    Private btnAdicionar As Button
    Private btnAtualizar As Button
    Private btnEliminar As Button
    Private btnLimpar As Button
    Private btnPesquisar As Button
    Private txtPesquisa As TextBox
    Private lblId As Label
    Private idSelecionado As Integer = 0
    
    Public Sub New()
        InitializeComponent()
    End Sub
    
    Private Sub InitializeComponent()
        ' Configurar formulário
        Me.Text = "Gestão de Livros"
        Me.Size = New Size(900, 650)
        Me.StartPosition = FormStartPosition.CenterScreen
        
        ' Label ID (oculto)
        lblId = New Label()
        lblId.Text = "ID: "
        lblId.Location = New Point(20, 20)
        lblId.Size = New Size(200, 25)
        Me.Controls.Add(lblId)
        
        ' Label Título
        Dim lblTitulo As New Label()
        lblTitulo.Text = "Título:"
        lblTitulo.Location = New Point(20, 50)
        lblTitulo.Size = New Size(60, 25)
        Me.Controls.Add(lblTitulo)
        
        ' TextBox Título
        txtTitulo = New TextBox()
        txtTitulo.Location = New Point(100, 50)
        txtTitulo.Size = New Size(250, 25)
        Me.Controls.Add(txtTitulo)
        
        ' Label Autor
        Dim lblAutor As New Label()
        lblAutor.Text = "Autor:"
        lblAutor.Location = New Point(370, 50)
        lblAutor.Size = New Size(60, 25)
        Me.Controls.Add(lblAutor)
        
        ' TextBox Autor
        txtAutor = New TextBox()
        txtAutor.Location = New Point(450, 50)
        txtAutor.Size = New Size(200, 25)
        Me.Controls.Add(txtAutor)
        
        ' Label Ano
        Dim lblAno As New Label()
        lblAno.Text = "Ano:"
        lblAno.Location = New Point(20, 90)
        lblAno.Size = New Size(60, 25)
        Me.Controls.Add(lblAno)
        
        ' TextBox Ano
        txtAno = New TextBox()
        txtAno.Location = New Point(100, 90)
        txtAno.Size = New Size(100, 25)
        Me.Controls.Add(txtAno)
        
        ' Label Estado
        Dim lblEstado As New Label()
        lblEstado.Text = "Estado:"
        lblEstado.Location = New Point(220, 90)
        lblEstado.Size = New Size(60, 25)
        Me.Controls.Add(lblEstado)
        
        ' ComboBox Estado
        cmbEstado = New ComboBox()
        cmbEstado.Location = New Point(300, 90)
        cmbEstado.Size = New Size(150, 25)
        cmbEstado.Items.Add("Disponível")
        cmbEstado.Items.Add("Emprestado")
        cmbEstado.SelectedIndex = 0
        Me.Controls.Add(cmbEstado)
        
        ' Botão Adicionar
        Dim btnAdicionar = New Button()
        btnAdicionar.Text = "Adicionar"
        btnAdicionar.Location = New Point(20, 130)
        btnAdicionar.Size = New Size(100, 30)
        AddHandler btnAdicionar.Click, AddressOf BtnAdicionar_Click
        Me.Controls.Add(btnAdicionar)
        
        ' Botão Atualizar
        Dim btnAtualizar = New Button()
        btnAtualizar.Text = "Atualizar"
        btnAtualizar.Location = New Point(130, 130)
        btnAtualizar.Size = New Size(100, 30)
        AddHandler btnAtualizar.Click, AddressOf BtnAtualizar_Click
        Me.Controls.Add(btnAtualizar)
        
        ' Botão Eliminar
        Dim btnEliminar = New Button()
        btnEliminar.Text = "Eliminar"
        btnEliminar.Location = New Point(240, 130)
        btnEliminar.Size = New Size(100, 30)
        AddHandler btnEliminar.Click, AddressOf BtnEliminar_Click
        Me.Controls.Add(btnEliminar)
        
        ' Botão Limpar
        Dim btnLimpar = New Button()
        btnLimpar.Text = "Limpar"
        btnLimpar.Location = New Point(350, 130)
        btnLimpar.Size = New Size(100, 30)
        AddHandler btnLimpar.Click, AddressOf BtnLimpar_Click
        Me.Controls.Add(btnLimpar)
        
        ' Label Pesquisa
        Dim lblPesquisa As New Label()
        lblPesquisa.Text = "Pesquisar:"
        lblPesquisa.Location = New Point(550, 130)
        lblPesquisa.Size = New Size(80, 30)
        Me.Controls.Add(lblPesquisa)
        
        ' TextBox Pesquisa
        txtPesquisa = New TextBox()
        txtPesquisa.Location = New Point(630, 130)
        txtPesquisa.Size = New Size(150, 25)
        Me.Controls.Add(txtPesquisa)
        
        ' Botão Pesquisar
        Dim btnPesquisar = New Button()
        btnPesquisar.Text = "Pesquisar"
        btnPesquisar.Location = New Point(790, 130)
        btnPesquisar.Size = New Size(80, 30)
        AddHandler btnPesquisar.Click, AddressOf BtnPesquisar_Click
        Me.Controls.Add(btnPesquisar)
        
        ' DataGridView
        dgvLivros = New DataGridView()
        dgvLivros.Location = New Point(20, 180)
        dgvLivros.Size = New Size(850, 400)
        dgvLivros.AllowUserToAddRows = False
        AddHandler dgvLivros.CellClick, AddressOf DgvLivros_CellClick
        Me.Controls.Add(dgvLivros)
    End Sub
    
    Private Sub FormLivros_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CarregarLivros()
    End Sub
    
    Private Sub CarregarLivros()
        Try
            dgvLivros.DataSource = Nothing
            Dim livros = LivroDAL.Listar()
            
            If livros.Count > 0 Then
                Dim dt As New DataTable()
                dt.Columns.Add("ID")
                dt.Columns.Add("Título")
                dt.Columns.Add("Autor")
                dt.Columns.Add("Ano")
                dt.Columns.Add("Estado")
                
                For Each livro In livros
                    dt.Rows.Add(livro.Id, livro.Titulo, livro.Autor, livro.Ano, livro.Estado)
                Next
                
                dgvLivros.DataSource = dt
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao carregar livros: " & ex.Message)
        End Try
    End Sub
    
    Private Sub BtnAdicionar_Click(sender As Object, e As EventArgs)
        Dim validacao = Validacoes.ValidarLivro(txtTitulo.Text, txtAutor.Text, txtAno.Text)
        If validacao <> "" Then
            MessageBox.Show(validacao, "Erro de Validação")
            Return
        End If
        
        Dim livro As New Livro()
        livro.Titulo = txtTitulo.Text
        livro.Autor = txtAutor.Text
        livro.Ano = CInt(txtAno.Text)
        livro.Estado = cmbEstado.SelectedItem.ToString()
        
        If LivroDAL.Inserir(livro) Then
            MessageBox.Show("Livro adicionado com sucesso!", "Sucesso")
            BtnLimpar_Click(Nothing, Nothing)
            CarregarLivros()
        Else
            MessageBox.Show("Erro ao adicionar livro", "Erro")
        End If
    End Sub
    
    Private Sub BtnAtualizar_Click(sender As Object, e As EventArgs)
        If idSelecionado = 0 Then
            MessageBox.Show("Selecione um livro para atualizar", "Aviso")
            Return
        End If
        
        Dim validacao = Validacoes.ValidarLivro(txtTitulo.Text, txtAutor.Text, txtAno.Text)
        If validacao <> "" Then
            MessageBox.Show(validacao, "Erro de Validação")
            Return
        End If
        
        Dim livro As New Livro()
        livro.Id = idSelecionado
        livro.Titulo = txtTitulo.Text
        livro.Autor = txtAutor.Text
        livro.Ano = CInt(txtAno.Text)
        livro.Estado = cmbEstado.SelectedItem.ToString()
        
        If LivroDAL.Atualizar(livro) Then
            MessageBox.Show("Livro atualizado com sucesso!", "Sucesso")
            BtnLimpar_Click(Nothing, Nothing)
            CarregarLivros()
        Else
            MessageBox.Show("Erro ao atualizar livro", "Erro")
        End If
    End Sub
    
    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs)
        If idSelecionado = 0 Then
            MessageBox.Show("Selecione um livro para eliminar", "Aviso")
            Return
        End If
        
        If MessageBox.Show("Tem certeza que deseja eliminar este livro?", "Confirmação", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            If LivroDAL.Eliminar(idSelecionado) Then
                MessageBox.Show("Livro eliminado com sucesso!", "Sucesso")
                BtnLimpar_Click(Nothing, Nothing)
                CarregarLivros()
            Else
                MessageBox.Show("Erro ao eliminar livro", "Erro")
            End If
        End If
    End Sub
    
    Private Sub BtnLimpar_Click(sender As Object, e As EventArgs)
        txtTitulo.Clear()
        txtAutor.Clear()
        txtAno.Clear()
        cmbEstado.SelectedIndex = 0
        txtPesquisa.Clear()
        lblId.Text = "ID: "
        idSelecionado = 0
        CarregarLivros()
    End Sub
    
    Private Sub BtnPesquisar_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtPesquisa.Text) Then
            CarregarLivros()
            Return
        End If
        
        Try
            dgvLivros.DataSource = Nothing
            Dim livros = LivroDAL.PesquisarPorTitulo(txtPesquisa.Text)
            
            If livros.Count > 0 Then
                Dim dt As New DataTable()
                dt.Columns.Add("ID")
                dt.Columns.Add("Título")
                dt.Columns.Add("Autor")
                dt.Columns.Add("Ano")
                dt.Columns.Add("Estado")
                
                For Each livro In livros
                    dt.Rows.Add(livro.Id, livro.Titulo, livro.Autor, livro.Ano, livro.Estado)
                Next
                
                dgvLivros.DataSource = dt
            Else
                MessageBox.Show("Nenhum livro encontrado", "Informação")
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao pesquisar livros: " & ex.Message)
        End Try
    End Sub
    
    Private Sub DgvLivros_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row = dgvLivros.Rows(e.RowIndex)
            idSelecionado = CInt(row.Cells("ID").Value)
            txtTitulo.Text = row.Cells("Título").Value.ToString()
            txtAutor.Text = row.Cells("Autor").Value.ToString()
            txtAno.Text = row.Cells("Ano").Value.ToString()
            cmbEstado.SelectedItem = row.Cells("Estado").Value.ToString()
            lblId.Text = "ID: " & idSelecionado.ToString()
        End If
    End Sub
End Class