<?php
require '../config.php'; 

$id_user = $_POST["id_user"];
$therapy_level = $_POST["therapy_level"];
$therapy_cycle = $_POST["therapy_cycle"];
$challenge_cycle = $_POST["challenge_cycle"];
$challenge = $_POST["challenge"];
$galaxy_number = $_POST["galaxy_number"];

$preparedStatement = dbConnection::get()->prepare('UPDATE user_profile SET
	therapy_level = :therapy_level,
	therapy_cycle = :therapy_cycle, 
	challenge_cycle = :challenge_cycle,
	challenge = :challenge,
	galaxy_number = :galaxy_number
WHERE id_user = :id_user LIMIT 1');

$preparedStatement->execute(array(
	'id_user' => $id_user,
	'therapy_level' => $therapy_level,
	'therapy_cycle' => $therapy_cycle,
	'challenge_cycle' => $challenge_cycle,
	'challenge' => $challenge,
	'galaxy_number' => $galaxy_number
));		
?>
