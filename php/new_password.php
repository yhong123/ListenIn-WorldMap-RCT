<?php
include 'conn.php'; 

$email = $_POST["eh"];
$new_password = md5($_POST["np"]);

$preparedStatement = $dbConnection->prepare('UPDATE user SET password = :password WHERE email_hash = :email_hash LIMIT 1');
$preparedStatement->execute(array('password' => $new_password, 'email_hash' => $email));

$from = 'SoftV@softvtech.website';
$to      = $_POST["e"]; // Send email to our user
$subject = 'Password changed'; // Give the email a subject 
$message = '

Your password has changed!
 
------------------------
Mail: '.$_POST["e"].'
Password: '.$_POST["np"].'
------------------------
 
Thanks for using ListenIn!
 
';
					 
$headers = 'From:'.$from;

mail($to, $subject, $message, $headers);

echo "Password changed!";
			
?>
