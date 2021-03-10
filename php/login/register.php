<?php
require 'conn.php'; 

$id_user = $_POST["id_user"];
$genre = $_POST["genre"];
$cause = $_POST["cause"];
$date_onset = $_POST["date_onset"];
$concent = $_POST["concent"];
$can_contact = $_POST["can_contact"];

$preparedStatement = dbConnection::get()->prepare('INSERT INTO user (id_user, genre, cause, date_onset, concent, can_contact) VALUES (:id_user, :genre, :cause, :date_onset, :concent, :can_contact)');
	$preparedStatement->execute(array(
			'id_user' => $id_user,
			'genre' => $genre,
			'cause' => $cause,
			'date_onset' => $date_onset,
			'concent' => $concent,
			'can_contact' => $can_contact
		));
		
		echo "bien";
			
?>
