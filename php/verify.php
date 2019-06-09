<?php
include 'conn.php'; 

if(isset($_GET['hash_e']) && !empty($_GET['hash_e']) AND isset($_GET['hash_p']) && !empty($_GET['hash_p']))
{
    // Verify data
    $email = mysql_real_escape_string($_GET['hash_e']); // Set email variable
    $password = mysql_real_escape_string($_GET['hash_p']); // Set hash variable
	
	$preparedStatement = $dbConnection->prepare('UPDATE user SET email_verified = :email_verified WHERE email_hash = :email AND password = :password LIMIT 1');
	$preparedStatement->execute(array('email' => $email, 'password' => $password, 'email_verified' => 'true'));
	echo "Email verified";
}
			
?>
