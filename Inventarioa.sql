-- DATABASE SORTU
CREATE DATABASE Inventarioa;
USE Inventarioa;
-- MINTEGIA
CREATE TABLE Mintegia (
    id_mintegia INT PRIMARY KEY,
    izena VARCHAR(100)
);

-- ERABILTZAILEA
CREATE TABLE Erabiltzailea (
    id_erabiltzailea INT PRIMARY KEY,
    izena VARCHAR(100),
    email VARCHAR(100),
    pasahitza VARCHAR(100),
    rola VARCHAR(50)
);

-- GAILUA
CREATE TABLE Gailua (
    id_gailua INT PRIMARY KEY,
    marka VARCHAR(100),
    erosketa_data DATE,
    kokalekua VARCHAR(100),
    egoera VARCHAR(50),
    id_mintegia INT,
    FOREIGN KEY (id_mintegia) REFERENCES Mintegia(id_mintegia)
);

-- N:M ERLAZIOA (Erabiltzen du)
CREATE TABLE ErabiltzenDu (
    id_erabiltzailea INT,
    id_gailua INT,
    PRIMARY KEY (id_erabiltzailea, id_gailua),
    FOREIGN KEY (id_erabiltzailea) REFERENCES Erabiltzailea(id_erabiltzailea),
    FOREIGN KEY (id_gailua) REFERENCES Gailua(id_gailua)
);

-- ORDENAGAILUA (ESPEZIALIZAZIOA)
CREATE TABLE Ordenagailua (
    gailua_id INT PRIMARY KEY,
    prozesagailua VARCHAR(100),
    ram VARCHAR(50),
    FOREIGN KEY (gailua_id) REFERENCES Gailua(id_gailua)
);

-- INPRIMAGAILUA (ESPEZIALIZAZIOA)
CREATE TABLE Inprimagailua (
    gailua_id INT PRIMARY KEY,
    koloretakoa BOOLEAN,
    FOREIGN KEY (gailua_id) REFERENCES Gailua(id_gailua)
);

-- EZABATUTAKO GAILUA (trigger bidez sortzeko)
CREATE TABLE EzabatutakoGailua (
    id_gailua INT,
    marka VARCHAR(100),
    erosketa_data DATE,
    kokalekua VARCHAR(100),
    ezabatze_data TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- MINTEGIA
INSERT INTO Mintegia (id_mintegia, izena) VALUES
(M01, 'Informatika Mintegia'),
(M02, 'Mekanika Mintegia');

-- ERABILTZAILEA
INSERT INTO Erabiltzailea (id_erabiltzailea, izena, email, pasahitza, rola) VALUES
(001, 'Ane Agirre', 'amanterola@ikastetxea.eus', '12345678', 'Irakaslea'),
(002, 'Jon Agirretxe', 'iktarduraduna@ikastetxea.eus', '87654321', 'IKT arduraduna'),
(003, 'Mikel Goikoetxea', 'mintegiburua@ikastetxea.eus', '76543219', 'Mintegi burua');

-- GAILUA
INSERT INTO Gailua (id_gailua, marka, erosketa_data, kokalekua, egoera, id_mintegia) VALUES
(G01, 'Dell', '2020-01-10', '1. gela', 'Aktibo', 1),
(G02, 'HP', '2019-05-20', '2. gela', 'Hondatuta', 2),
(G03, 'Lenovo', '2021-03-15', 'Bulegoa', 'Hondatuta', 1);

-- ORDENAGAILUA
INSERT INTO Ordenagailua (gailua_id, prozesagailua, ram) VALUES
(G01, 'Intel i7', '16GB'),
(G03, 'Intel i5', '8GB');

-- INPRIMAGAILUA
INSERT INTO Inprimagailua (gailua_id, koloretakoa) VALUES
(G02, TRUE);

-- EZABATUTAKO GAILUA (normalean trigger-ak betetzen du, baina eskuz adibidea)
INSERT INTO EzabatutakoGailua (id_gailua, marka, erosketa_data, kokalekua, ezabatze_data) VALUES
(G04, 'Epson', '2020-01-01', 'Biltegia', CURRENT_TIMESTAMP);
