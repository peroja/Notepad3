-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Erstellungszeit: 05. Aug 2025 um 08:59
-- Server-Version: 10.6.22-MariaDB-cll-lve-log
-- PHP-Version: 8.3.23

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `randdlfq_registeredusers`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `registry`
--

CREATE TABLE `registry` (
  `id` int(5) NOT NULL,
  `regkey1` varchar(4) NOT NULL,
  `regkey2` varchar(4) NOT NULL,
  `regkey3` varchar(4) NOT NULL,
  `regkey4` varchar(4) NOT NULL,
  `name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `programm` varchar(255) NOT NULL,
  `registerdate` varchar(255) NOT NULL,
  `activated` int(1) NOT NULL,
  `notiz` int(1) NOT NULL,
  `regkey_combined` varchar(50) DEFAULT NULL,
  `regkey` varchar(255) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `registry`
--
ALTER TABLE `registry`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `regkey` (`regkey1`),
  ADD UNIQUE KEY `uk_regkeys` (`regkey1`,`regkey2`,`regkey3`,`regkey4`),
  ADD UNIQUE KEY `unique_regkey` (`regkey1`,`regkey2`,`regkey3`,`regkey4`),
  ADD UNIQUE KEY `regkey_combined` (`regkey_combined`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `registry`
--
ALTER TABLE `registry`
  MODIFY `id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
