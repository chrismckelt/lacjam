######################
# Configure an XML node or attribute, with a namespace table
######################
function Config([string] $xpath, [string] $value, [object] $ns) {
   
	$nodes = $doc.SelectNodes($xpath, $ns)
	
	foreach ($node in $nodes) {
		if ($node -ne $null) {
			if ($node.NodeType -eq "Element") {
				$node.InnerXml = $value
			}
			else {
				$node.Value = $value
			}
		}
	}
}

$doc = New-Object System.Xml.XmlDocument;
$doc.Load('bin/nhibernate.config');
$ns = New-Object Xml.XmlNamespaceManager $doc.NameTable
$ns.AddNamespace( "nh", "urn:nhibernate-configuration-2.2" )
$root = $doc.get_DocumentElement();
Config "//nh:property[@name='connection.connection_string']" 'data source=structerre.csae43ljslde.ap-southeast-2.rds.amazonaws.com;Initial Catalog=Lacjam;User Id=MetastoreUser;Password=MetastorePassword123;Connection Timeout=180;' $ns
$doc.Save('bin/nhibernate.config');

$doc = New-Object System.Xml.XmlDocument;
$doc.Load('nhibernate.config');
$ns = New-Object Xml.XmlNamespaceManager $doc.NameTable
$ns.AddNamespace( "nh", "urn:nhibernate-configuration-2.2" )
$root = $doc.get_DocumentElement();
Config "//nh:property[@name='connection.connection_string']" 'data source=structerre.csae43ljslde.ap-southeast-2.rds.amazonaws.com;Initial Catalog=Lacjam;User Id=MetastoreUser;Password=MetastorePassword123;Connection Timeout=180;' $ns
$doc.Save('nhibernate.config');