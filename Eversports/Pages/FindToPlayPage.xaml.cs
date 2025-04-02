<? php
require_once 'db.php';

function GetLookingToPlay($country, $city, $Dates, $FromTimes, $ToTimes, $choosenSports)
{
    $conn = openConnection();
    if ($conn->connect_error) {
        die("Connection failed: ". $conn->connect_error);
    }

    // Prepare the base query
    $query = "SELECT * FROM lookingtoplay WHERE country = ? AND city = ?";

    // Add filters for Dates, FromTimes, and ToTimes
    if (!empty($Dates))
    {
        $query.= " AND id IN (SELECT lookingtoplay_id FROM availabledatetime WHERE Date IN ('".implode("','", $Dates). "'))";
    }
    if (!empty($FromTimes))
    {
        $query.= " AND id IN (SELECT lookingtoplay_id FROM availabledatetime WHERE FromTime IN ('".implode("','", $FromTimes). "'))";
    }
    if (!empty($ToTimes))
    {
        $query.= " AND id IN (SELECT lookingtoplay_id FROM availabledatetime WHERE ToTime IN ('".implode("','", $ToTimes). "'))";
    }

    $stmt = $conn->prepare($query);
    $stmt->bind_param("ss", $country, $city);
    $stmt->execute();
    $result = $stmt->get_result();

    $xml = new SimpleXMLElement('<root/>');

    if ($result->num_rows > 0) {
        // Loop through each row from the "lookingtoplay" table
        while ($row = $result->fetch_assoc()) {
            $item = $xml->addChild('item');
            foreach ($row as $key => $value) {
                $item->addChild($key, htmlspecialchars($value));
            }
            // Query for availabledatetimes related to the current lookingtoplay entry
            $stmt2 = $conn->prepare("SELECT * FROM availabledatetime WHERE lookingtoplay_id = ?");
            $stmt2->bind_param("i", $row['id']);
            $stmt2->execute();
            $result2 = $stmt2->get_result();
            $item2 = $item->addChild('availabledatetimes');

            // Loop through the availabledatetimes result and add it to the XML
            while ($row2 = $result2->fetch_assoc()) {
                $item3 = $item2->addChild('availabledatetime');
                foreach ($row2 as $key => $value) {
                    $item3->addChild($key, htmlspecialchars($value));
                }
            }
            // Query for choosensport related to the current lookingtoplay entry
            $stmt3 = $conn->prepare("SELECT * FROM choosensport WHERE lookingtoplay_id = ?");
            $stmt3->bind_param("i", $row['id']);
            $stmt3->execute();
            $result3 = $stmt3->get_result();
            $item2 = $item->addChild('choosenSports');

            // Loop through the choosensport result and add it to the XML
            while ($row3 = $result3->fetch_assoc()) {
                $item3 = $item2->addChild('sport');
                foreach ($row3 as $key => $value) {
                    $item3->addChild($key, htmlspecialchars($value));
                }
            }
        }

        $stmt2->close();
        $stmt3->close();
        echo json_encode(["status" => "success", "obj" => $xml->asXML()]);
    } else
    {
        echo json_encode(["status" => "error", "obj" => "No results found"]);
        return; // Exit the function if no results are found
    }

    // Close all prepared statements and the database connection
    $stmt->close();
    $conn->close();
}
?>
