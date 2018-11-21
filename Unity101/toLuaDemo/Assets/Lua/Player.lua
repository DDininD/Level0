Player = {}
local this = Player
local rigid
local speed
local jumpforce
local transform
local club
function this.Start(obj,_speed,_jumpforce,_club)
    rigid = obj:GetComponent('Rigidbody2D')
    transform = obj:GetComponent('Transform')
    speed = _speed;
    jumpforce = _jumpforce;
    club = _club
end

local function Move()
    h = Input.GetAxis('Horizontal')
    rigid:AddForce(Vector2(h,0)*speed)
end
local function Jump()
    if Input.GetKeyDown(KeyCode.Space) then
        rigid:AddForce(Vector2(0,1)*jumpforce,ForceMode2D.Impulse)
    end
end
local function FCamera()
    local camera = Camera.main
    camera.transform.position = transform.position + Vector3(0,0,-10)
end
local function CheckPlayerAlive()
    if transform.position.y <= -5 then
        return false
    end
    return true
end

function this.Update()
    if GameState == -1 then
        return
    end
    Move()
    Jump()
    FCamera()
    if Input.GetMouseButtonDown(0) then
        club:SetActive(true)
    end
    if hitover then
        club:SetActive(false)
        hitover = false
    end
    if not CheckPlayerAlive() then
        GameOver(false)
    end

end


function this.CheckCollision(other)
    print(other.gameObject.name)
    if other.gameObject.name == 'spike' then
        GameOver(false)        
    end
    if other.gameObject.name == 'flag(Clone)' then
        GameOver(true)
    end
    if other.gameObject.name == 'club' then
        print("hit")
        rigid:AddForce(Vector2(100,100)*(UIs.barMask.fillAmount*5))
    end

end

