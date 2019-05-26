<?php
include 'conn.php'; 

$email = $_POST["email"];
$password = $_POST["password"];
$genre = $_POST["genre"];
$date_of_birth = $_POST["date_of_birth"];
$cause = $_POST["cause"];
$date_of_onset = $_POST["date_of_onset"];

//ASK FOR ALREADY USED EMAIL
$preparedStatement = $dbConnection->prepare('SELECT * FROM user WHERE email = :email');

$preparedStatement->execute(array('email' => $email));

if($preparedStatement->rowCount() > 0)
{
	echo "used";
}
else
{
	$preparedStatement = $dbConnection->prepare('INSERT INTO user (email, password, genre, date_of_birth, cause, date_of_onset) VALUES (:email, :password, :genre, :date_of_birth, :cause, :date_of_onset)');
	$preparedStatement->execute(array('email' => $email, 'password' => $password, 'genre' => $genre, 'date_of_birth' => $date_of_birth, 'cause' => $cause, 'date_of_onset' => $date_of_onset));
	echo "bien";
}
			
?>
