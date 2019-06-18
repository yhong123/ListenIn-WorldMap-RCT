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
	$subject = 'Signup | Verification'; // Give the email a subject 
	$message = '
 
	Thanks for signing up!
	Your account has been created, you can login with the following credentials after you have activated your account by pressing the url below.
	 
	------------------------
	Mail: '.$email.'
	------------------------
	 
	Please click this link to activate your account:
	http://softvtech.website/ListenIn/php/verify.php?hash_e='.$email_hash .'
	 
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
