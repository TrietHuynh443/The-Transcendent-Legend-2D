using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterOne : BaseScene
{

    // Start is called before the first frame update
    private void Start()
    {
        if(!isInit)
        {
            isInit = true;
            _gameManagerObject = Instantiate(_gameManagerPrefabs);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
