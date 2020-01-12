<?php
require 'conn.php'; 

$email = $_POST["email"];
$email_hash = $_POST["email_hash"];
$email_encrypted = $_POST["email_encrypted"];
$password = $_POST["password"];
$password_hash = $_POST["password_hash"];
$genre = $_POST["genre"];
$cause = $_POST["cause"];
$date_of_onset = $_POST["date_of_onset"];
$concent = $_POST["concent"];
$can_contact = $_POST["can_contact"];

//ASK FOR ALREADY USED EMAIL
$preparedStatement = dbConnection::get()->prepare('SELECT * FROM user WHERE email_hash = :email_hash');

$preparedStatement->execute(array('email_hash' => $email_hash));

if($preparedStatement->rowCount() > 0)
{
	echo "used";
}
else
{
	$preparedStatement = dbConnection::get()->prepare('INSERT INTO user (email_hash, email_encrypted, password, genre, cause, date_of_onset, concent, can_contact) VALUES (:email_hash, :email_encrypted, :password, :genre, :cause, :date_of_onset, :concent, :can_contact)');
	$preparedStatement->execute(array(
			'email_hash' => $email_hash,
			'email_encrypted' => $email_encrypted,
			'password' => $password_hash,
			'genre' => $genre,
			'cause' => $cause,
			'date_of_onset' => $date_of_onset,
			'concent' => $concent,
			'can_contact' => $can_contact
		));

	$to      = $email; // Send email to our user
	$subject = 'Signup | Verification'; // Give the email a subject 
	$message = '
 
	Thanks for signing up!
	Your account has been created, you can login with the following credentials after you have activated your account by pressing the url below.
	 
	------------------------
	Mail: '.$email.'
	Password: '.$password.'
	------------------------
	 
	Please click this link to activate your account:
	http://softvtech.website/ListenIn/php/verify.php?hash_e='.$email_hash.'
	 
	';
						 
	$headers = 'MIME-Version: 1.0' . '\r\n';
	$headers .= 'Content-type:text/html;charset=UTF-8' . '\r\n';
	$headers .= 'From: <noreply@ucl.ac.uk>' . '\r\n';

	mail($to, $subject, $message, $headers);

	echo "bien";
}
			
?>
