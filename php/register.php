<?php
include 'conn.php'; 

$email = $_POST["email"];
$email_hash = $_POST["email_hash"];
$email_encrypted = $_POST["email_encrypted"];
$password = $_POST["password"];
$password_hash = $_POST["password_hash"];
$genre = $_POST["genre"];
$date_of_birth = $_POST["date_of_birth"];
$cause = $_POST["cause"];
$date_of_onset = $_POST["date_of_onset"];
$concent = $_POST["concent"];

//ASK FOR ALREADY USED EMAIL
$preparedStatement = $dbConnection->prepare('SELECT * FROM user WHERE email_hash = :email_hash');

$preparedStatement->execute(array('email_hash' => $email_hash));

if($preparedStatement->rowCount() > 0)
{
	echo "used";
}
else
{
	$preparedStatement = $dbConnection->prepare('INSERT INTO user (email_hash, email_encrypted, password, genre, date_of_birth, cause, date_of_onset, concent) VALUES (:email_hash, :email_encrypted, :password, :genre, :date_of_birth, :cause, :date_of_onset, :concent)');
	$preparedStatement->execute(array(
			'email_hash' => $email_hash,
			'email_encrypted' => $email_encrypted,
			'password' => $password_hash,
			'genre' => $genre,
			'date_of_birth' => $date_of_birth,
			'cause' => $cause,
			'date_of_onset' => $date_of_onset,
			'concent' => $concent
		));
		
	$from = 'SoftV@softvtech.website';
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
						 
	$headers = 'From:'.$from;

	mail($to, $subject, $message, $headers);

	echo "bien";
}
			
?>
