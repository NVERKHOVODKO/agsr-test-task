{
	"info": {
		"_postman_id": "7c721098-fb12-437c-9b4f-514ee8be6e44",
		"name": "Patient API Demo",
		"description": "Collection for demonstrating Patient API methods",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
		"_exporter_id": "31992583"
	},
	"item": [
		{
			"name": "Search Patients by BirthDate",
			"item": [
				{
					"name": "Equal to date (eq)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=eq1985-05-15",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "eq1985-05-15"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Not equal to date (ne)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=ne1985-05-15",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "ne1985-05-15"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Less than date (lt)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=lt1990-01-01",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "lt1990-01-01"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Greater than date (gt)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=gt1980-01-01",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "gt1980-01-01"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Less or equal (le)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=le1985-05-15",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "le1985-05-15"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Greater or equal (ge)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=ge1985-05-15",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "ge1985-05-15"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Starts after (sa)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=sa1985-05-15",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "sa1985-05-15"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Ends before (eb)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=eb1985-05-15",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "eb1985-05-15"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Approximately (ap)",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{base_url}}/api/patients/search?date=ap1985-05-15",
							"host": [
								"{{base_url}}"
							],
							"path": [
								"api",
								"patients",
								"search"
							],
							"query": [
								{
									"key": "date",
									"value": "ap1985-05-15"
								}
							]
						}
					},
					"response": []
				}
			]
		},
		{
			"name": "Add Patient",
			"request": {
				"method": "POST",
				"header": [
					{
						"key": "Content-Type",
						"value": "application/json"
					}
				],
				"body": {
					"mode": "raw",
					"raw": "{\n  \"name\": {\n    \"family\": \"Smith\",\n    \"given\": [\"John\", \"Michael\"],\n    \"use\": \"official\"\n  },\n  \"gender\": 1,\n  \"birthDate\": \"1985-05-15T00:00:00\",\n  \"active\": true\n}"
				},
				"url": {
					"raw": "{{base_url}}/api/patients",
					"host": [
						"{{base_url}}"
					],
					"path": [
						"api",
						"patients"
					]
				}
			},
			"response": []
		},
		{
			"name": "Get All Patients",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "{{base_url}}/api/patients",
					"host": [
						"{{base_url}}"
					],
					"path": [
						"api",
						"patients"
					]
				}
			},
			"response": []
		},
		{
			"name": "Get Patient by ID",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "{{base_url}}/api/patients/{{patient_id}}",
					"host": [
						"{{base_url}}"
					],
					"path": [
						"api",
						"patients",
						"{{patient_id}}"
					]
				}
			},
			"response": []
		},
		{
			"name": "Update Patient",
			"request": {
				"method": "PUT",
				"header": [
					{
						"key": "Content-Type",
						"value": "application/json"
					}
				],
				"body": {
					"mode": "raw",
					"raw": "{\n  \"id\": \"{{patient_id}}\",\n  \"name\": {\n    \"family\": \"Smith-Jones\",\n    \"given\": [\"John\", \"Michael\"],\n    \"use\": \"official\"\n  },\n  \"gender\": 1,\n  \"birthDate\": \"1985-05-15T00:00:00\",\n  \"active\": false\n}"
				},
				"url": {
					"raw": "{{base_url}}/api/patients/{{patient_id}}",
					"host": [
						"{{base_url}}"
					],
					"path": [
						"api",
						"patients",
						"{{patient_id}}"
					]
				}
			},
			"response": []
		},
		{
			"name": "Delete Patient",
			"request": {
				"method": "DELETE",
				"header": [],
				"url": {
					"raw": "{{base_url}}/api/patients/{{patient_id}}",
					"host": [
						"{{base_url}}"
					],
					"path": [
						"api",
						"patients",
						"{{patient_id}}"
					]
				}
			},
			"response": []
		}
	],
	"event": [
		{
			"listen": "prerequest",
			"script": {
				"type": "text/javascript",
				"packages": {},
				"exec": [
					""
				]
			}
		},
		{
			"listen": "test",
			"script": {
				"type": "text/javascript",
				"packages": {},
				"exec": [
					""
				]
			}
		}
	],
	"variable": [
		{
			"key": "base_url",
			"value": "http://localhost:7272"
		},
		{
			"key": "patient_id",
			"value": ""
		}
	]
}
