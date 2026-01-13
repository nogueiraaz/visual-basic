-- Criar base de dados
CREATE DATABASE IF NOT EXISTS livraria;
USE livraria;

-- Tabela Utilizadores
CREATE TABLE utilizadores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    turma VARCHAR(50) NOT NULL,
    contacto VARCHAR(20) NOT NULL,
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela Livros
CREATE TABLE livros (
    id INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(150) NOT NULL,
    autor VARCHAR(100) NOT NULL,
    ano INT NOT NULL,
    estado ENUM('Disponível', 'Emprestado') DEFAULT 'Disponível',
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela Emprestimos
CREATE TABLE emprestimos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    id_livro INT NOT NULL,
    id_utilizador INT NOT NULL,
    data_emprestimo DATE NOT NULL,
    data_devolucao DATE,
    status ENUM('Ativo', 'Devolvido') DEFAULT 'Ativo',
    CONSTRAINT fk_livro FOREIGN KEY (id_livro) REFERENCES livros(id),
    CONSTRAINT fk_utilizador FOREIGN KEY (id_utilizador) REFERENCES utilizadores(id)
);

-- Dados de teste
INSERT INTO utilizadores (nome, turma, contacto) VALUES
('João Silva', '10A', '912345678'),
('Maria Santos', '10B', '923456789'),
('Carlos Oliveira', '11A', '934567890');

INSERT INTO livros (titulo, autor, ano, estado) VALUES
('Dom Casmurro', 'Machado de Assis', 2001, 'Disponível'),
('O Cortiço', 'Aluísio Azevedo', 1990, 'Disponível'),
('Memórias Póstumas de Brás Cubas', 'Machado de Assis', 1985, 'Emprestado');