<?php
require '../game_con/config.php';
//$checksumPHP = crc32($file_game);
//$checksumC = $_POST['checksum'];

$checksumPHP = 22;
$checksumC = 22;

if($_POST['file_size'] == $_FILES['file_data']['size'])
{
	//DATA
	$id_user = $_POST['id_user'];
	$file_name = $_POST['file_name'];

	$root_directory = '../../files/'.$id_user.'/therapy/ACT/';

	$audio_path = $root_directory.$_FILES['file_data']['name'];
	
	if (!file_exists($root_directory))
	{
		$oldmask = umask(0);
		mkdir($root_directory, 0775, true);
		umask($oldmask);
	}
	//files
	move_uploaded_file($_FILES['file_data']['tmp_name'], $audio_path);
	chmod($audio_path, 0664);
	
	echo $_POST['file_size']. ' == ' .$_FILES['file_data']['size'];
}
else
{
	echo "checksum_error";
}
?>
