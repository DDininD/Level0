


function Init(o1,o2,o3,o4,o5,o6,o7,o8)
    GameObject = UnityEngine.GameObject
    Transform = UnityEngine.Transform
    Input = UnityEngine.Input
    KeyCode = UnityEngine.KeyCode
    Time = UnityEngine.Time
    Camera = UnityEngine.Camera
    Resources = UnityEngine.Resources
    ForceMode2D = UnityEngine.ForceMode2D
    Collider2D = UnityEngine.Collider2D
    Random = UnityEngine.Random
    SpriteRenderer = UnityEngine.SpriteRenderer
    SceneManager = UnityEngine.SceneManagement.SceneManager
    DOTween = DG.Tweening.DOTween
    LoopType = DG.Tweening.LoopType
    Image = UnityEngine.UI.Image
    GameState = 1 
    UIs = {}
    gs = {}
    mapLength = 0
    flag = nil

    
    
    UIs.win = o1.gameObject
    UIs.lose = o2.gameObject
    UIs.barMask = o8:GetComponent(typeof(Image))
    
    gs[1] = o3.gameObject
    gs[2] = o4.gameObject
    gs[3] = o5.gameObject
    flag = o6.gameObject
    
    mapLength = o7
    print(gs)
    
    UIManager.AddEventToAButton(UIs.win,function()
        SceneManager.LoadScene("game")
    end)
    UIManager.AddEventToAButton(UIs.lose,function()
        SceneManager.LoadScene("game")
    end)
end

function GameOver(isWin)
    if GameState == -1 then
        return
    end
    if isWin then
        UIManager.SetActive(UIs.win,true)
    else
        UIManager.SetActive(UIs.lose,true)
    end
    GameState = -1
end


