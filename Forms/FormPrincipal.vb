Imports System.Windows.Forms
Imports System.Drawing
Public Class FormPrincipal
    Inherits Form
    
    Public Sub New()
        InitializeComponent()
    End Sub
    
    Private Sub InitializeComponent()
        Me.Text = "Sistema de Gestão de Biblioteca Escolar"
        Me.Size = New Size(600, 400)
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub
    
    Private Sub FormPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If DatabaseConnection.TestarConexao() Then
            MessageBox.Show("Conexão com a base de dados estabelecida com sucesso!", "Sucesso")
        Else
            MessageBox.Show("Erro ao conectar com a base de dados!", "Erro")
        End If
        
        CriarMenu()
    End Sub
    
    Private Sub CriarMenu()
        Dim menuStrip As New MenuStrip()
        
        Dim menuFicheiro As New ToolStripMenuItem("Ficheiro")
        menuFicheiro.DropDownItems.Add("Sair", Nothing, Sub() Me.Close())
        
        Dim menuGestao As New ToolStripMenuItem("Gestão")
        menuGestao.DropDownItems.Add("Utilizadores", Nothing, AddressOf MenuUtilizadores)
        menuGestao.DropDownItems.Add("Livros", Nothing, AddressOf MenuLivros)
        menuGestao.DropDownItems.Add("Empréstimos", Nothing, AddressOf MenuEmprestimos)
        
        Dim menuAjuda As New ToolStripMenuItem("Ajuda")
        menuAjuda.DropDownItems.Add("Sobre", Nothing, AddressOf MenuSobre)
        
        menuStrip.Items.Add(menuFicheiro)
        menuStrip.Items.Add(menuGestao)
        menuStrip.Items.Add(menuAjuda)
        
        Me.MainMenuStrip = menuStrip
        Me.Controls.Add(menuStrip)
    End Sub
    
    Private Sub MenuUtilizadores()
        MessageBox.Show("Gestão de Utilizadores - Não implementado ainda", "Info")
    End Sub
    
    Private Sub MenuLivros()
        MessageBox.Show("Gestão de Livros - Não implementado ainda", "Info")
    End Sub
    
    Private Sub MenuEmprestimos()
        MessageBox.Show("Gestão de Empréstimos - Não implementado ainda", "Info")
    End Sub
    
    Private Sub MenuSobre()
        MessageBox.Show("Sistema de Gestão de Biblioteca Escolar v1.0" & vbCrLf & 
                        "Desenvolvido em VB.NET com MySQL", "Sobre")
    End Sub
End Class