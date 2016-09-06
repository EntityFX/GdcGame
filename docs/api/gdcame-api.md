Authorization:
	Login:
		Request:
			Url: http://localhost:9001/token/
			Method: Post
			Content-Type: application/x-www-form-urlencoded
			Body: 
				grant_type=password
				username=<login>
				password=<password>
				
		Response:
			Status: 200
			Content-Type: application/json;charset=UTF-8
			Content:
				{
					"access_token": "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAtVit-dVluUaJiGLeiVn7IAAAAAACAAAAAAADZgAAwAAAABAAAAB5GQYiKecGPxW_SnNeRWPkAAAAAASAAACgAAAAEAAAACLz4Vb5jEJFmdJTTCiCKCi4AAAAZPBiJ2pbtHI04646lWXJ7-J5y5nQx2C1pA4R5UeO0aMt80li4peg7H-LfbSGLoYGiBDD9wrjoo0Nn5eo_if-0RRuCYsNawrILx7lZqvtqnodoUovhkO0WfGLxB8y-P-qAHpk1lCjhg-7Vlz39MD-V2LmfeX2Zp2AZhzB5DIJlGp3-hhNHWuVyMb-B0kOEIb9sbaxzL4sUH46e-QY5N3vyJn5aOneYkpR7k6rkOTRG-THELSQii2F3xQAAABffFNQlRN5VUZ6qb20KL4RVUDyKA"
					"token_type": "bearer"
					"expires_in": 1799
				}
	
	Register:
		Request:
			Url: http://localhost:9001/api/auth/register
			Method: Post
			Content-Type: application/x-www-form-urlencoded
			Body: 
				Email=<login>
				Password=<password>
				ConfirmPassword=<password>
			
		Response:
			Status: 201

	
Heartbeat:
	Request:
		Url: http://localhost:9001/api/heartbeat
		Method: Get
		Headers:
			Authorization: Bearer <access_token>
		
	Response:
		Status: 200
		Content-Type: application/json;charset=UTF-8
		Content:
			{ }
			
Account:
	Get user by id:
		Request:
			Url: http://localhost:9001/api/accounts/<user-id>
			Method: Get
			Headers:
				Authorization: Bearer <access_token>
			
		Response:
			Status: 200
			Content-Type: application/json;charset=UTF-8
			Content:
				{
				  "userId": "ee7674105b14a26568638be62db6fae6",
				  "login": "entityfx"
				}
		
	Find users:
		Request:
			Url: http://localhost:9001/api/accounts
				http://localhost:9001/api/accounts/filter/<search-string>
			Method: Get
			Headers:
				Authorization: Bearer <access_token>
				
		Response:
			Status: 200
			Content-Type: application/json;charset=UTF-8
			Content:
				[
				  {
					"userId": "646c02acfb1d549dc66af26e4c511783",
					"login": "12355"
				  },
				  {
					"userId": "b508464edbc10e0fd2e25a5d79dd4bcc",
					"login": "1"
				  }
				]
				
	Delete user:
		Request:
			Url: http://localhost:9001/api/accounts/<user-id>
			Method: Delete
			Headers:
				Authorization: Bearer <access_token>
		
		Response:
			Status: 204
			
			
Game api:
	Get game data:
		Request:
			Url: http://localhost:9001/api/game/game-data
			Method: Get
			Headers:
				Authorization: Bearer <access_token>
				
		Response:
			Status: 200
			Content-Type: application/json;charset=UTF-8
			Content:
				{
				  "fundsDrivers": [
					{...}
				  ],
				  "counters": {
					"currentFunds": 100,
					"totalFunds": 100,
					"counters": [ ...    ]
				  }
				}
				
	Get game counters:
		Request:
			Url: http://localhost:9001/api/game/game-counters
			Method: Get
			Headers:
				Authorization: Bearer <access_token>
				
		Response:
			Status: 200
			Content-Type: application/json;charset=UTF-8
			Content:
				{
				  "currentFunds": 100,
				  "totalFunds": 100,
				  "counters": [
					{
					  "id": 0,
					  "name": "GDC Points",
					  "value": 0,
					  "type": 0
					},
					{
					  "bonus": 0,
					  "bonusPercentage": 0,
					  "inflation": 0,
					  "subValue": 10,
					  "id": 1,
					  "name": "Профессионализм",
					  "value": 10,
					  "type": 1
					},
					{
					  "bonus": 0,
					  "bonusPercentage": 0,
					  "inflation": 2,
					  "subValue": 0,
					  "id": 2,
					  "name": "Зарплата",
					  "value": 0,
					  "type": 1
					},
					{
					  "secondsRemaining": 0,
					  "unlockValue": 10000,
					  "id": 3,
					  "name": "Квартальный план",
					  "value": 5000000,
					  "type": 2
					}
				  ]
		}
		
	Perform step:
		Request:
			Url: http://localhost:9001/api/game/perform-step
			Method: Post
			Content-Type: application/x-www-form-urlencoded
			
		Response:
			Status: 200
			
		Addon: may asc to enter numbers