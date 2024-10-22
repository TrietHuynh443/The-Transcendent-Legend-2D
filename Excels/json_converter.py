import subprocess
import sys

import json

def install_package(package):
    subprocess.check_call(["pip", "install", package])
try:
    import pandas as pd
except ImportError:
    install_package('pandas')
    import pandas as pd


# Read all sheets from the Excel file
excel_data_df = pd.read_excel('GameData.xlsx', sheet_name=None)

# excel_data_df = excel_data_df.dropna(how='all', axis=1)
# Iterate through each sheet in the Excel file
for sheet_name, sheet_data in excel_data_df.items():
    # Convert each sheet to JSON format (define orientation of document, here 'records')
    # sheet_data.set_index(sheet_data.columns[0], inplace=True)
    
    thisisjson = sheet_data.to_json(orient='records')

    thisisjson_dict = json.loads(thisisjson)
    
    # Define the JSON file name based on the sheet name
    json_file_name = f'{sheet_name}.json'
    
    thisisjson_dict = json.loads(thisisjson)

    # Save the JSON data to a file
    with open(json_file_name, 'w') as json_file:
        json.dump(thisisjson_dict, json_file)

    print(f'{sheet_name} data saved to {json_file_name}')
