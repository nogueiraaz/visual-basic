Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data
Public Class FormUtilizadores
    Inherits Form
    
    Private dgvUtilizadores As DataGridView
    Private txtNome As TextBox
    Private txtTurma As TextBox
    Private txtContacto As TextBox
    Private btnAdicionar As Button
    Private btnAtualizar As Button
    Private btnEliminar As Button
    Private btnLimpar As Button
    Private lblId As Label
    Private idSelecionado As Integer = 0
    
    Public Sub New()
        InitializeComponent()
    End Sub
    
    Private Sub InitializeComponent()
        ' Configurar formulário
        Me.Text = "Gestão de Utilizadores"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        
        ' Label ID (oculto)
        lblId = New Label()
        lblId.Text = "ID: "
        lblId.Location = New Point(20, 20)
        lblId.Size = New Size(200, 25)
        Me.Controls.Add(lblId)
        
        ' Label Nome
        Dim lblNome As New Label()
        lblNome.Text = "Nome:"
        lblNome.Location = New Point(20, 50)
        lblNome.Size = New Size(60, 25)
        Me.Controls.Add(lblNome)
        
        ' TextBox Nome
        txtNome = New TextBox()
        txtNome.Location = New Point(100, 50)
        txtNome.Size = New Size(200, 25)
        Me.Controls.Add(txtNome)
        
        ' Label Turma
        Dim lblTurma As New Label()
        lblTurma.Text = "Turma:"
        lblTurma.Location = New Point(320, 50)
        lblTurma.Size = New Size(60, 25)
        Me.Controls.Add(lblTurma)
        
        ' TextBox Turma
        txtTurma = New TextBox()
        txtTurma.Location = New Point(400, 50)
        txtTurma.Size = New Size(100, 25)
        Me.Controls.Add(txtTurma)
        
        ' Label Contacto
        Dim lblContacto As New Label()
        lblContacto.Text = "Contacto:"
        lblContacto.Location = New Point(20, 90)
        lblContacto.Size = New Size(60, 25)
        Me.Controls.Add(lblContacto)
        
        ' TextBox Contacto
        txtContacto = New TextBox()
        txtContacto.Location = New Point(100, 90)
        txtContacto.Size = New Size(200, 25)
        Me.Controls.Add(txtContacto)
        
        ' Botão Adicionar
        btnAdicionar = New Button()
        btnAdicionar.Text = "Adicionar"
        btnAdicionar.Location = New Point(20, 130)
        btnAdicionar.Size = New Size(100, 30)
        AddHandler btnAdicionar.Click, AddressOf BtnAdicionar_Click
        Me.Controls.Add(btnAdicionar)
        
        ' Botão Atualizar
        btnAtualizar = New Button()
        btnAtualizar.Text = "Atualizar"
        btnAtualizar.Location = New Point(130, 130)
        btnAtualizar.Size = New Size(100, 30)
        AddHandler btnAtualizar.Click, AddressOf BtnAtualizar_Click
        Me.Controls.Add(btnAtualizar)
        
        ' Botão Eliminar
        btnEliminar = New Button()
        btnEliminar.Text = "Eliminar"
        btnEliminar.Location = New Point(240, 130)
        btnEliminar.Size = New Size(100, 30)
        AddHandler btnEliminar.Click, AddressOf BtnEliminar_Click
        Me.Controls.Add(btnEliminar)
        
        ' Botão Limpar
        btnLimpar = New Button()
        btnLimpar.Text = "Limpar"
        btnLimpar.Location = New Point(350, 130)
        btnLimpar.Size = New Size(100, 30)
        AddHandler btnLimpar.Click, AddressOf BtnLimpar_Click
        Me.Controls.Add(btnLimpar)
        
        ' DataGridView
        dgvUtilizadores = New DataGridView()
        dgvUtilizadores.Location = New Point(20, 180)
        dgvUtilizadores.Size = New Size(750, 350)
        dgvUtilizadores.AllowUserToAddRows = False
        AddHandler dgvUtilizadores.CellClick, AddressOf DgvUtilizadores_CellClick
        Me.Controls.Add(dgvUtilizadores)
    End Sub
    
    Private Sub FormUtilizadores_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CarregarUtilizadores()
    End Sub
    
    Private Sub CarregarUtilizadores()
        Try
            dgvUtilizadores.DataSource = Nothing
            Dim utilizadores = UtilizadorDAL.Listar()
            
            If utilizadores.Count > 0 Then
                ' Criar tabela de dados
                Dim dt As New DataTable()
                dt.Columns.Add("ID")
                dt.Columns.Add("Nome")
                dt.Columns.Add("Turma")
                dt.Columns.Add("Contacto")
                
                For Each util In utilizadores
                    dt.Rows.Add(util.Id, util.Nome, util.Turma, util.Contacto)
                Next
                
                dgvUtilizadores.DataSource = dt
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao carregar utilizadores: " & ex.Message)
        End Try
    End Sub
    
    Private Sub BtnAdicionar_Click(sender As Object, e As EventArgs)
        ' Validar campos
        Dim validacao = Validacoes.ValidarUtilizador(txtNome.Text, txtTurma.Text, txtContacto.Text)
        If validacao <> "" Then
            MessageBox.Show(validacao, "Erro de Validação")
            Return
        End If
        
        ' Criar novo utilizador
        Dim utilizador As New Utilizador()
        utilizador.Nome = txtNome.Text
        utilizador.Turma = txtTurma.Text
        utilizador.Contacto = txtContacto.Text
        
        If UtilizadorDAL.Inserir(utilizador) Then
            MessageBox.Show("Utilizador adicionado com sucesso!", "Sucesso")
            BtnLimpar_Click(Nothing, Nothing)
            CarregarUtilizadores()
        Else
            MessageBox.Show("Erro ao adicionar utilizador", "Erro")
        End If
    End Sub
    
    Private Sub BtnAtualizar_Click(sender As Object, e As EventArgs)
        If idSelecionado = 0 Then
            MessageBox.Show("Selecione um utilizador para atualizar", "Aviso")
            Return
        End If
        
        ' Validar campos
        Dim validacao = Validacoes.ValidarUtilizador(txtNome.Text, txtTurma.Text, txtContacto.Text)
        If validacao <> "" Then
            MessageBox.Show(validacao, "Erro de Validação")
            Return
        End If
        
        ' Atualizar utilizador
        Dim utilizador As New Utilizador()
        utilizador.Id = idSelecionado
        utilizador.Nome = txtNome.Text
        utilizador.Turma = txtTurma.Text
        utilizador.Contacto = txtContacto.Text
        
        If UtilizadorDAL.Atualizar(utilizador) Then
            MessageBox.Show("Utilizador atualizado com sucesso!", "Sucesso")
            BtnLimpar_Click(Nothing, Nothing)
            CarregarUtilizadores()
        Else
            MessageBox.Show("Erro ao atualizar utilizador", "Erro")
        End If
    End Sub
    
    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs)
        If idSelecionado = 0 Then
            MessageBox.Show("Selecione um utilizador para eliminar", "Aviso")
            Return
        End If
        
        If MessageBox.Show("Tem certeza que deseja eliminar este utilizador?", "Confirmação", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            If UtilizadorDAL.Eliminar(idSelecionado) Then
                MessageBox.Show("Utilizador eliminado com sucesso!", "Sucesso")
                BtnLimpar_Click(Nothing, Nothing)
                CarregarUtilizadores()
            Else
                MessageBox.Show("Erro ao eliminar utilizador", "Erro")
            End If
        End If
    End Sub
    
    Private Sub BtnLimpar_Click(sender As Object, e As EventArgs)
        txtNome.Clear()
        txtTurma.Clear()
        txtContacto.Clear()
        lblId.Text = "ID: "
        idSelecionado = 0
    End Sub
    
    Private Sub DgvUtilizadores_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row = dgvUtilizadores.Rows(e.RowIndex)
            idSelecionado = CInt(row.Cells("ID").Value)
            txtNome.Text = row.Cells("Nome").Value.ToString()
            txtTurma.Text = row.Cells("Turma").Value.ToString()
            txtContacto.Text = row.Cells("Contacto").Value.ToString()
            lblId.Text = "ID: " & idSelecionado.ToString()
        End If
    End Sub
End Class