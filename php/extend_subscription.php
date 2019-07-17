<?php
include 'conn.php'; 

$id_user = $_POST["id_user"];
$period = $_POST["period"];
$current_time = date_create(date('Y-m-d h:i:s', time()));

$preparedStatement = $dbConnection->prepare('SELECT * FROM user WHERE id = :id_user LIMIT 1');

$preparedStatement->execute(array('id_user' => $id_user));

if($preparedStatement->rowCount() > 0)
{
	$result = $preparedStatement->fetch();

	$current_time = $current_time->format('Y-m-d h:i:s');
	
	if($period == 1)
	{
		$subscription = date('Y-m-d h:i:s', strtotime($current_time . ' + 30 days'));
	}
	elseif($period == 6)
	{
		$subscription = date('Y-m-d h:i:s', strtotime($current_time . ' + 180 days'));
	}
	
	$preparedStatement = $dbConnection->prepare('UPDATE user SET subscription_end = :subscription_end WHERE id = :id_user LIMIT 1');
	$preparedStatement->execute(array('subscription_end' => $subscription, 'id_user' => $id_user));
	echo "bien";
}
else
{
	echo "error";
}
?>
