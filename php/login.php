<?php
include 'conn.php'; 

$id = $_POST["id"];

//ASK FOR THE SITES
$preparedStatement = $dbConnection->prepare('SELECT * FROM user WHERE id = :id');

$preparedStatement->execute(array('id' => $id));

if($preparedStatement->rowCount() > 0)
{
	$row = $preparedStatement -> fetch();
	echo $row['password'];
}
else
{
	echo "false";
}
			
?>
