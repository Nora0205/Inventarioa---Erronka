# Instalazio Gida - StockBaseApp

Gida honek StockBaseApp aplikazioa ingurune lokal batean nola instalatu eta konfiguratu azaltzen du.

## 1. Baldintzak
Aplikazioa exekutatzeko honako softwarea beharrezkoa da:
*   **.NET 10.0 SDK** edo berriagoa.
*   **MySQL Server 8.0** edo MariaDB.
*   **MySQL Workbench** edo **DBeaver** (Datu-basea kudeatzeko).

## 2. Datu-basearen Konfigurazioa
1.  Ireki zure SQL kudeatzailea (DBeaver adibidez).
2.  Sortu datu-base berri bat `Inventarioa` izenarekin.
3.  Exekutatu proiektuaren erroan dagoen `Inventarioa_Final.sql` script-a taulak eta hasierako datuak sortzeko.
4.  Ziurtatu MySQL erabiltzailea `root` dela eta pasahitza `root` dela (edo aldatu `Konexioa.cs` fitxategian).

## 3. Aplikazioaren Konfigurazioa
1.  Klonatu edo deskargatu proiektua GitHub-etik.
2.  Ireki `StockBaseApp.slnx` Visual Studio-rekin.
3.  Egiaztatu NuGet paketeak instalatuta daudela:
    *   `MySql.Data`
4.  Konpilatu proiektua (`Build Solution`).

## 4. Erabiltzaileak
Sisteman sartzeko erabili honako kredentzial hauetako bat:
*   **Admin**: `mzubizarreta@ikastetxea.eus` / `maite123`
*   **Irakaslea**: `ketxeberria@ikastetxea.eus` / `koldo456`
