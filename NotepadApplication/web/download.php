<?php
// Datenbankverbindung
$host = 'localhost';
$dbname = 'your dbname';
$user = 'your username';
$pass = 'your password';
$dsn = "mysql:host=$host;dbname=$dbname;charset=utf8mb4";

try {
    $pdo = new PDO($dsn, $user, $pass);
} catch (PDOException $e) {
    die("Datenbankverbindung fehlgeschlagen: " . $e->getMessage());
}

// Falls ein Download-Request vorliegt
if (isset($_GET['download_id'])) {
    $downloadId = intval($_GET['download_id']);
    $stmt = $pdo->prepare("SELECT * FROM downloads WHERE id = ?");
    $stmt->execute([$downloadId]);
    $row = $stmt->fetch(PDO::FETCH_ASSOC);

    if ($row) {
        // IP Adresse erfassen
        $ip = $_SERVER['REMOTE_ADDR'];
        $ipLog = trim($row['ip_log'] . "\n" . date("Y-m-d H:i:s") . " - " . $ip);

        // Zähler erhöhen & IP loggen
        $update = $pdo->prepare("UPDATE downloads SET downloads = downloads + 1, last_download = NOW(), ip_log = ? WHERE id = ?");
        $update->execute([$ipLog, $downloadId]);

        // Weiterleitung zur eigentlichen Datei
        header("Location: " . $row['download_url']);
        exit;
    }
}

// Daten abrufen
$stmt = $pdo->query("SELECT id, programm, version, download_url, downloads FROM downloads ORDER BY version DESC");
$downloads = $stmt->fetchAll(PDO::FETCH_ASSOC);
?>

<!DOCTYPE html>
<html lang="de">
<head>
    <meta charset="UTF-8">
    <title>Download Übersicht</title>
    <style>
        body { font-family: sans-serif; background: white; padding: 2rem; }
        table { border-collapse: collapse; width: 100%; max-width: 800px; margin: auto; }
        th, td { border: 1px solid #ccc; padding: 10px; text-align: left; }
        th { background-color: #f9f9f9; }
    </style>
</head>
<body>
    <h2 style="text-align:center;">Download Übersicht</h2>
    <table>
        <tr>
            <th>Programm</th>
            <th>Version</th>
            <th>Download</th>
            <th>Downloaded</th>
        </tr>
        <?php foreach ($downloads as $row): ?>
        <tr>
            <td><?= htmlspecialchars($row['programm']) ?></td>
            <td><?= htmlspecialchars($row['version']) ?></td>
            <td><a href="?download_id=<?= $row['id'] ?>">Download</a></td>
            <td><?= (int)$row['downloads'] ?></td>
        </tr>
        <?php endforeach; ?>
    </table>
</body>
</html>
