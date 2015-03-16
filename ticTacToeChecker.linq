<Query Kind="Program" />

void Main()
{
	var bothWon = new List<List<int>> { new List<int> { 1, 1, 1 }, 
								 	    new List<int> { 1, 2, 1 },
									    new List<int> { 2, 2, 2 } };
									 
	var playerOneInvalid = new List<List<int>> { new List<int> { 1, 1, 1 },
									 			 new List<int> { 2, 1, 2 },
									 			 new List<int> { 2, 1, 2 } };
									 
	var nobodyWon = new List<List<int>> { new List<int> { 2, 2, 0 }, 
										  new List<int> { 1, 0, 1 }, 
									 	  new List<int> { 1, 2, 1 } };
									 
	var playerOneWonR = new List<List<int>> { new List<int> { 2, 2, 0 }, 
					 						  new List<int> { 1, 1, 1 }, 
											  new List<int> { 1, 2, 1 } };
									 
	var playerTwoWonC = new List<List<int>> { new List<int> { 1, 2, 0 }, 
											  new List<int> { 1, 2, 0 }, 
											  new List<int> { 0, 2, 1 } };
									 
	var playerOneWonD = new List<List<int>> { new List<int> { 1, 2, 0 }, 
											  new List<int> { 2, 1, 2 }, 
											  new List<int> { 2, 2, 1 } };
											  
	var playerOneWonR4 = new List<List<int>> { new List<int> { 2, 2, 0, 2 }, 
					 						   new List<int> { 1, 1, 1, 1 }, 
											   new List<int> { 1, 2, 1, 0 },
											   new List<int> { 2, 1, 0, 0 } };
									 
	var playerTwoWonC4 = new List<List<int>> { new List<int> { 1, 2, 0, 1 }, 
											   new List<int> { 1, 2, 0, 0 }, 
											   new List<int> { 0, 2, 1, 2 },
											   new List<int> { 0, 2, 2, 1 } };
									 
	var playerOneWonD4 = new List<List<int>> { new List<int> { 1, 2, 0, 1 }, 
											   new List<int> { 2, 1, 2, 0 }, 
											   new List<int> { 2, 2, 1, 2 },
											   new List<int> { 2, 2, 1, 1 }};
	
	new TicTacToe(bothWon).WhoIsWinner();
	//new TicTacToe(playerOneInvalid).WhoIsWinner();
	new TicTacToe(nobodyWon).WhoIsWinner();
	
	new TicTacToe(playerOneWonR).WhoIsWinner();
	new TicTacToe(playerTwoWonC).WhoIsWinner();
	new TicTacToe(playerOneWonD).WhoIsWinner();
	
	new TicTacToe(playerOneWonR4).WhoIsWinner();
	new TicTacToe(playerTwoWonC4).WhoIsWinner();
	new TicTacToe(playerOneWonD4).WhoIsWinner();
}

// Define other methods and classes here

class TicTacToe {
	List<List<int>> _rows = new List<List<int>>();
	List<List<int>> _cols = new List<List<int>>();
	List<List<int>> _diags = new List<List<int>>();

	internal TicTacToe(List<List<int>> ttt) {
		_rows = ttt;		
		_cols = Transpose(_rows);
		_diags = ExtractDiags(_rows);
	}
	
	static List<List<int>> ExtractDiags(List<List<int>> rows) {
		var diags = new List<List<int>>();
		var diagR = new List<int>();
		for(var i = 0; i < rows.Count; i++)		
			for(var j = 0; j < rows.Count; j++)
				if(i == j) 
					diagR.Add( rows[i][j] );
		diags.Add(diagR);
		
		var diagL = new List<int>();
		for(var i = 0; i < rows.Count; i++)
			for(var j = rows.Count - 1; j >= 0; j--)
				if(i + j == rows.Count - 1) 
					diagL.Add( rows[i][j] );
		diags.Add(diagL);
		
		return diags;
	}
	
	static List<List<int>> Transpose(List<List<int>> rows) {
		var cols = new List<List<int>>();
		for(var i = 0; i < rows.Count; i++) {
			var col = new List<int>();
			for(var j = 0; j < rows.Count; j++)
				col.Add( rows[j][i] );
			cols.Add(col);
		}
		return cols;
	}
	
	static void IsValid(bool rowsChecked, bool colsChecked, bool diagsChecked) {
		if((rowsChecked && colsChecked) 
			|| (colsChecked && diagsChecked)
			|| (rowsChecked && diagsChecked)
			|| (rowsChecked && colsChecked && diagsChecked)) {
			"Your TicTacToe is broken :(".Dump();
			throw new Exception();
		}
	}
	
	bool IsPlayerWinner(int mark) {
		var rowsChecked = _rows.Any(r => r.All(v => v == mark));
		var colsChecked = _cols.Any(c => c.All(v => v == mark));
		var diagsChecked = _diags.Any(d => d.All(v => v == mark));	
		
		IsValid(rowsChecked, colsChecked, diagsChecked); // bad
			
		return rowsChecked || colsChecked || diagsChecked;
	}
	
	bool IsPlayerOneWinner() {
		return IsPlayerWinner(1);
	}
	
	bool IsPlayerTwoWinner() {
		return IsPlayerWinner(2);
	}
	
	internal void WhoIsWinner() {
		if (IsPlayerOneWinner() && IsPlayerTwoWinner()) {
			"Both players won o_O".Dump();
			return;
		}
		if (IsPlayerOneWinner()) 
			"Player 1 won :)".Dump();
		else if (IsPlayerTwoWinner()) 
			"Player 2 won ;)".Dump();
		else 
			"Nobody won :|".Dump();
	}
}