<?php
include 'conn.php'; 

$email_hash = $_POST["email_hash"];
$email = $_POST["email"];

//ASK FOR ALREADY USED EMAIL
$preparedStatement = $dbConnection->prepare('SELECT * FROM user WHERE email_hash = :email_hash');

$preparedStatement->execute(array('email_hash' => $email_hash));

if($preparedStatement->rowCount() > 0)
{
	$from = 'SoftV@softvtech.website';
	$to      = $email; // Send email to our user
	$subject = 'Change password'; // Give the email a subject 
	$message = '
 
	Please click this link to reset your password:
	http://softvtech.website/ListenIn/php/reset_password_form.php?eh='.$email_hash .'&e='.$email.'
	 
	';
						 
	$headers = 'From:'.$from;

	mail($to, $subject, $message, $headers);

	echo "bien";
}
else
{
	echo "mal";
}
			
?>
