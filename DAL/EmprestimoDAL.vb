Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
' Classe responsável pelas operações de base de dados para Empréstimos
Public Class EmprestimoDAL
    
    ' Registar novo empréstimo
    Public Shared Function Inserir(emprestimo As Emprestimo) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                Dim query As String = "INSERT INTO emprestimos (id_livro, id_utilizador, data_emprestimo, status) " &
                                      "VALUES (@idLivro, @idUtilizador, @dataEmprestimo, @status)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@idLivro", emprestimo.IdLivro)
                    cmd.Parameters.AddWithValue("@idUtilizador", emprestimo.IdUtilizador)
                    cmd.Parameters.AddWithValue("@dataEmprestimo", emprestimo.DataEmprestimo)
                    cmd.Parameters.AddWithValue("@status", "Ativo")
                    
                    cmd.ExecuteNonQuery()
                    
                    ' Atualizar estado do livro para Emprestado
                    AtualizarEstadoLivro(emprestimo.IdLivro, "Emprestado")
                    
                    Return True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao registar empréstimo: " & ex.Message)
            Return False
        End Try
    End Function
    
    ' Listar empréstimos ativos
    Public Shared Function Listar() As List(Of Emprestimo)
        Dim emprestimos As New List(Of Emprestimo)
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return emprestimos
                
                Dim query As String = "SELECT * FROM emprestimos WHERE status = 'Ativo'"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim emp As New Emprestimo()
                            emp.Id = reader.GetInt32("id")
                            emp.IdLivro = reader.GetInt32("id_livro")
                            emp.IdUtilizador = reader.GetInt32("id_utilizador")
                            emp.DataEmprestimo = reader.GetDateTime("data_emprestimo")
                            If Not IsDBNull(reader("data_devolucao")) Then
                                emp.DataDevolucao = reader.GetDateTime("data_devolucao")
                            End If
                            emp.Status = reader.GetString("status")
                            emprestimos.Add(emp)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao listar empréstimos: " & ex.Message)
        End Try
        Return emprestimos
    End Function
    
    ' Registar devolução de um livro
    Public Shared Function RegistarDevolucao(id As Integer) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                ' Obter ID do livro para atualizar seu estado
                Dim idLivro As Integer = 0
                Dim query As String = "SELECT id_livro FROM emprestimos WHERE id = @id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        idLivro = CInt(result)
                    End If
                End Using
                
                ' Atualizar empréstimo com status Devolvido
                query = "UPDATE emprestimos SET status = 'Devolvido', data_devolucao = @dataDevolucao WHERE id = @id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.Parameters.AddWithValue("@dataDevolucao", Now.Date)
                    cmd.ExecuteNonQuery()
                End Using
                
                ' Atualizar estado do livro para Disponível
                AtualizarEstadoLivro(idLivro, "Disponível")
                
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao registar devolução: " & ex.Message)
            Return False
        End Try
    End Function
    
    ' Método privado para atualizar o estado de um livro
    Private Shared Function AtualizarEstadoLivro(idLivro As Integer, estado As String) As Boolean
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return False
                
                Dim query As String = "UPDATE livros SET estado = @estado WHERE id = @id"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", idLivro)
                    cmd.Parameters.AddWithValue("@estado", estado)
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
    
    ' Listar todos os empréstimos (incluindo devolvidos)
    Public Shared Function ListarTodos() As List(Of Emprestimo)
        Dim emprestimos As New List(Of Emprestimo)
        Try
            Using conn = DatabaseConnection.GetConnection()
                If conn Is Nothing Then Return emprestimos
                
                Dim query As String = "SELECT * FROM emprestimos"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim emp As New Emprestimo()
                            emp.Id = reader.GetInt32("id")
                            emp.IdLivro = reader.GetInt32("id_livro")
                            emp.IdUtilizador = reader.GetInt32("id_utilizador")
                            emp.DataEmprestimo = reader.GetDateTime("data_emprestimo")
                            If Not IsDBNull(reader("data_devolucao")) Then
                                emp.DataDevolucao = reader.GetDateTime("data_devolucao")
                            End If
                            emp.Status = reader.GetString("status")
                            emprestimos.Add(emp)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao listar empréstimos: " & ex.Message)
        End Try
        Return emprestimos
    End Function
End Class