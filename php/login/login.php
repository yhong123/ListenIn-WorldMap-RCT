<?php
require 'conn.php'; 

$id_user = $_POST["id_user"];

//ASK FOR THE SITES
$preparedStatement = dbConnection::get()->prepare('SELECT * FROM user WHERE id_user = :id_user LIMIT 1');

$preparedStatement->execute(array('id_user' => $id_user));

if($preparedStatement->rowCount() > 0)
{
	echo 'exist';
}
else
{
	echo 'new_user';
}
			
?>
