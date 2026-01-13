Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
' Classe responsável pelas operações de base de dados para Livros
Public Class LivroDAL
    
    ' Inserir novo livro na base de dados
    Public Shared Function Inserir(livro As Livro) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                Dim query As String = "INSERT INTO livros (titulo, autor, ano, estado) VALUES (@titulo, @autor, @ano, @estado)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@titulo", livro.Titulo)
                    cmd.Parameters.AddWithValue("@autor", livro.Autor)
                    cmd.Parameters.AddWithValue("@ano", livro.Ano)
                    cmd.Parameters.AddWithValue("@estado", livro.Estado)
                    
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao inserir livro: " & ex.Message)
            Return False
        End Try
    End Function
    
    ' Listar todos os livros da base de dados
    Public Shared Function Listar() As List(Of Livro)
        Dim livros As New List(Of Livro)
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return livros
                
                Dim query As String = "SELECT * FROM livros"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim livro As New Livro()
                            livro.Id = reader.GetInt32("id")
                            livro.Titulo = reader.GetString("titulo")
                            livro.Autor = reader.GetString("autor")
                            livro.Ano = reader.GetInt32("ano")
                            livro.Estado = reader.GetString("estado")
                            livros.Add(livro)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao listar livros: " & ex.Message)
        End Try
        Return livros
    End Function
    
    ' Atualizar dados de um livro existente
    Public Shared Function Atualizar(livro As Livro) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                Dim query As String = "UPDATE livros SET titulo=@titulo, autor=@autor, ano=@ano, estado=@estado WHERE id=@id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", livro.Id)
                    cmd.Parameters.AddWithValue("@titulo", livro.Titulo)
                    cmd.Parameters.AddWithValue("@autor", livro.Autor)
                    cmd.Parameters.AddWithValue("@ano", livro.Ano)
                    cmd.Parameters.AddWithValue("@estado", livro.Estado)
                    
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao atualizar livro: " & ex.Message)
            Return False
        End Try
    End Function
    
    ' Eliminar um livro da base de dados
    Public Shared Function Eliminar(id As Integer) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                Dim query As String = "DELETE FROM livros WHERE id=@id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao eliminar livro: " & ex.Message)
            Return False
        End Try
    End Function
    
    ' Pesquisar livros por título
    Public Shared Function PesquisarPorTitulo(titulo As String) As List(Of Livro)
        Dim livros As New List(Of Livro)
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return livros
                
                Dim query As String = "SELECT * FROM livros WHERE titulo LIKE @titulo"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@titulo", "%" & titulo & "%")
                    
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim livro As New Livro()
                            livro.Id = reader.GetInt32("id")
                            livro.Titulo = reader.GetString("titulo")
                            livro.Autor = reader.GetString("autor")
                            livro.Ano = reader.GetInt32("ano")
                            livro.Estado = reader.GetString("estado")
                            livros.Add(livro)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao pesquisar livros: " & ex.Message)
        End Try
        Return livros
    End Function
    
    ' Pesquisar livros por autor
    Public Shared Function PesquisarPorAutor(autor As String) As List(Of Livro)
        Dim livros As New List(Of Livro)
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return livros
                
                Dim query As String = "SELECT * FROM livros WHERE autor LIKE @autor"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@autor", "%" & autor & "%")
                    
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim livro As New Livro()
                            livro.Id = reader.GetInt32("id")
                            livro.Titulo = reader.GetString("titulo")
                            livro.Autor = reader.GetString("autor")
                            livro.Ano = reader.GetInt32("ano")
                            livro.Estado = reader.GetString("estado")
                            livros.Add(livro)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao pesquisar livros: " & ex.Message)
        End Try
        Return livros
    End Function
End Class