<Query Kind="Program">
  <Reference>&lt;ProgramFilesX86&gt;\MongoDB\CSharpDriver 1.10-rc0\MongoDB.Bson.dll</Reference>
  <Namespace>MongoDB.Bson</Namespace>
</Query>

// In above "Language" dropdown select "C# Program" and hit F5

void Main()
{
	// init nodes
	var n = new ListNode();
	var m = new ListNode();
	var k = new ListNode();
	var j = new ListNode();
	
	// fill data
	n.Data = "you";	
	m.Data = "are so beautiful";	
	k.Data = "and I will";	
	j.Data = "wait for you forever";
	
	// init links
	n.Next = m;
	m.Next = k;
	k.Next = j;
	
	m.Prev = n;
	k.Prev = m;
	
	m.Rand = j;
	
	// prepare list
	var list = new ListRand();
	list.Head = n;
	list.Tail = j;
	list.Count = 4;
	
	// test serialization
	var filePath = @"C:\Users\Roman\My Projects\data_new.txt";
	using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
		list.Serialize(fs);
	}
	
	// test deserialization
	ListRand newList;
	var fileName = @"C:\Users\Roman\My Projects\data_new.txt";
	using (var fs = File.OpenRead(fileName)) {
		newList = ListRand.Deserialize(fs);
	}	
	
	// dump object graph, circular refs tolerant
	newList.Dump();	
}

// Define other methods and classes here
class ProtoListNode {
	public int PrevId;
	public int NextId;
	public int RandId;
	
	public ListNode Node;
}

class ListNode {
	public string Data;
	
	public ListNode Prev;
	public ListNode Next;	
	public ListNode Rand;
}

class ListRand
{
	public ListNode Head;
	public ListNode Tail;

	public int Count;

	public void Serialize(FileStream fs) {
    	using (var sw = new StreamWriter(fs, Encoding.UTF8)) {
			var dumpedList = new Dictionary<ListNode, int>();
			var ix = 1; // костыль
			
			var node = Head;
			while(node != null) {
				dumpedList.Add(node, ix);
				ix += 1;
				node = node.Next;
			}
			
			node = Head;
			while(node != null) {
				const string nullStr = "null";
				var prevNodeId = nullStr;
				var nextNodeId = nullStr;
				var randNodeId = nullStr;
				
				if (node.Prev != null) {
					prevNodeId = dumpedList[node.Prev].ToString();
				}
				if (node.Next != null) {
					nextNodeId = dumpedList[node.Next].ToString();
				}
				if (node.Rand != null) {
					randNodeId = dumpedList[node.Rand].ToString();
				}
			
				var line = string.Format("{0},{1},{2},{3},{4}", dumpedList[node], prevNodeId, nextNodeId, randNodeId, node.Data);
        		sw.WriteLine(line);
				
				node = node.Next;
			}
		}
	}

 	public static ListRand Deserialize(FileStream fs) {
		var protoNodesDict = new Dictionary<int, ProtoListNode> ();
		
		ParseFileToDict(fs, protoNodesDict);		
		FillNodes(protoNodesDict);
		
		return InitList(protoNodesDict);
	}
	
	private static void ParseFileToDict(FileStream fs, Dictionary<int, ProtoListNode> protoNodesDict) {
		using (var sr = new StreamReader(fs, Encoding.UTF8, true)) {
			string line;
			while ((line = sr.ReadLine()) != null) {
				var values = line.Split(new [] { ',' });
				
				var protoNode = new ProtoListNode();
				
				const string nullStr = "null";
				
				if (values[1] != nullStr) {
					protoNode.PrevId = Int32.Parse(values[1]);
				}	
				if (values[2] != nullStr) {
					protoNode.NextId = Int32.Parse(values[2]);
				}
				if (values[3] != nullStr) {
					protoNode.RandId = Int32.Parse(values[3]);
				}
	
				var protoNodeId = Int32.Parse(values[0]);
				
				var node = new ListNode();
				node.Data = values[4];
				
				protoNode.Node = node;
				
				protoNodesDict.Add(protoNodeId, protoNode);
			}			
		}
	}
	
	private static void FillNodes(Dictionary<int, ProtoListNode> protoNodesDict) {
		foreach (var pair in protoNodesDict) {
			var nextId = pair.Value.NextId;
			if (nextId != 0) { // костыль
    			pair.Value.Node.Next = protoNodesDict[nextId].Node;
			}
			
			var prevId = pair.Value.PrevId;
			if (prevId != 0) {
    			pair.Value.Node.Prev = protoNodesDict[prevId].Node;
			}
			
			var randId = pair.Value.RandId;
			if (randId != 0) {
    			pair.Value.Node.Rand = protoNodesDict[randId].Node;
			}
		}
	}
	
	private static ListRand InitList(Dictionary<int, ProtoListNode> protoNodesDict) {
		var list = new ListRand();		
		
		list.Head = protoNodesDict.Values.First().Node;
		list.Tail = protoNodesDict.Values.Last().Node;
		list.Count = protoNodesDict.Count;
		
		return list;
	}
}