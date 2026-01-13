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
        
        Dim lblTitulo As New Label()
        lblTitulo.Text = "Sistema de Gestão de Biblioteca"
        lblTitulo.Font = New Font("Arial", 14, FontStyle.Bold)
        lblTitulo.Location = New Point(50, 20)
        lblTitulo.Size = New Size(300, 30)
        lblTitulo.TextAlign = ContentAlignment.MiddleCenter
        Me.Controls.Add(lblTitulo)
        
        Dim lblEmail As New Label()
        lblEmail.Text = "Utilizador (ID ou Nome):"
        lblEmail.Location = New Point(30, 70)
        lblEmail.Size = New Size(150, 25)
        Me.Controls.Add(lblEmail)
        
        txtEmail = New TextBox()
        txtEmail.Location = New Point(30, 95)
        txtEmail.Size = New Size(340, 25)
        Me.Controls.Add(txtEmail)
        
        Dim lblPassword As New Label()
        lblPassword.Text = "Contacto (Password):"
        lblPassword.Location = New Point(30, 130)
        lblPassword.Size = New Size(150, 25)
        Me.Controls.Add(lblPassword)
        
        txtPassword = New TextBox()
        txtPassword.Location = New Point(30, 155)
        txtPassword.Size = New Size(340, 25)
        txtPassword.UseSystemPasswordChar = True
        Me.Controls.Add(txtPassword)
        
        btnEntrar = New Button()
        btnEntrar.Text = "Entrar"
        btnEntrar.Location = New Point(150, 200)
        btnEntrar.Size = New Size(100, 35)
        AddHandler btnEntrar.Click, AddressOf BtnEntrar_Click
        Me.Controls.Add(btnEntrar)
        
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
            Dim utilizadores = UtilizadorDAL.Listar()
            Dim utilizadorEncontrado As Utilizador = Nothing
            
            For Each util In utilizadores
                If util.Id.ToString() = txtEmail.Text OrElse util.Nome.ToLower() = txtEmail.Text.ToLower() Then
                    If util.Contacto = txtPassword.Text Then
                        utilizadorEncontrado = util
                        Exit For
                    End If
                End If
            Next
            
            If utilizadorEncontrado IsNot Nothing Then
                UtilizadorLogado = utilizadorEncontrado
                TipoUtilizador = "User"
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