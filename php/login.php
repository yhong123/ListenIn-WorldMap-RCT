<?php
include 'conn.php'; 

$email = $_POST["email"];
$password = $_POST["password"];

//ASK FOR THE SITES
$preparedStatement = $dbConnection->prepare('SELECT * FROM user WHERE email = :email and password = :password');

$preparedStatement->execute(array('email' => $email, 'password' => $password));

if($preparedStatement->rowCount() > 0)
{
	echo "true";
}
else
{
	echo "false";
}
			
?>
