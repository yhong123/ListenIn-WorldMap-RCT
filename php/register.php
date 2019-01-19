<?php
include 'conn.php'; 

$email = $_POST["email"];
$password = $_POST["password"];

//ASK FOR ALREADY USED EMAIL
$preparedStatement = $dbConnection->prepare('SELECT * FROM user WHERE email = :email');

$preparedStatement->execute(array('email' => $email));

if($preparedStatement->rowCount() > 0)
{
	echo "used";
}
else
{
	$preparedStatement = $dbConnection->prepare('INSERT INTO user (email, password) VALUES (:email, :password)');
	$preparedStatement->execute(array('email' => $email, 'password' => $password));
	echo "bien";
}
			
?>
