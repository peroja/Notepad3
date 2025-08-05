<?php
// Antwortformat als Klartext
header("Content-Type: text/plain");

// 📜 POST-Daten für Debug-Zwecke loggen
file_put_contents("ping.txt", date("Y-m-d H:i:s") . "\n" . json_encode($_POST, JSON_PRETTY_PRINT) . "\n\n", FILE_APPEND);

// 🔐 Datenbankverbindung vorbereiten
$host = 'localhost';
$dbname = 'your dbname';
$user = 'your username';
$pass = 'your password';
$dsn = "mysql:host=$host;dbname=$dbname;charset=utf8mb4";

try {
    $pdo = new PDO($dsn, $user, $pass);
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    echo "Datenbankverbindung fehlgeschlagen: " . $e->getMessage();
    exit;
}

// 🧩 Registrierung (alle Felder oder Teilmenge)
if (isset($_POST["regkey"], $_POST["email"], $_POST["programm"])) {
    $regkey = $_POST["regkey"];
    $name = $_POST["name"] ?? "Unbekannt";
    $email = $_POST["email"];
    $programm = $_POST["programm"];
    $registerdate = $_POST["registerdate"] ?? date("Y-m-d H:i:s");
    $notiz = $_POST["notiz"] ?? "auto";
    $activated = isset($_POST["activated"]) ? (int)$_POST["activated"] : 1;

    // 🔁 Insert or Update (wenn Key bereits existiert)
    $stmt = $pdo->prepare("
        INSERT INTO registry (regkey, name, email, programm, registerdate, activated, notiz)
        VALUES (:regkey, :name, :email, :programm, :registerdate, :activated, :notiz)
        ON DUPLICATE KEY UPDATE
            name = :name,
            email = :email,
            programm = :programm,
            registerdate = :registerdate,
            activated = :activated,
            notiz = :notiz
    ");

    try {
        $stmt->execute([
            ':regkey' => $regkey,
            ':name' => $name,
            ':email' => $email,
            ':programm' => $programm,
            ':registerdate' => $registerdate,
            ':activated' => $activated,
            ':notiz' => $notiz
        ]);
        echo "OK";
    } catch (PDOException $e) {
        echo "Fehler beim Speichern: " . $e->getMessage();
    }

    exit;
}

// 🚫 Keine gültige POST-Anfrage
echo "Ungültige Anfrage.";
exit;
?>
