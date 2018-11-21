Club = {}

local this = Club
local self
local transform
local hit = false
local colliders

hitover = false

function this.Start(obj)
    Club.transform = obj:GetComponent('Transform')
    colliders = obj:GetComponentsInChildren(typeof(Collider2D))
    transform = Club.transform
    self = obj
end





function this.Update()
    
    local v = Camera.main:ScreenToWorldPoint(Input.mousePosition)
    transform.position = Vector3(v.x,v.y,0)
    

    if Input.GetMouseButton(0) then
        if UIs.barMask.fillAmount < 1 then
            UIs.barMask.fillAmount = UIs.barMask.fillAmount + Time.deltaTime
        elseif UIs.barMask.fillAmount == 1 then 
            UIs.barMask.fillAmount = 0.05         
        end
    end

    if Input.GetMouseButtonUp(0) then
        hit = true
    end
    
    if hit == true then
        local oz = transform.rotation:ToEulerAngles().z
        print('oz'..tonumber(oz))
        local z = oz
        z = z + (UIs.barMask.fillAmount*7)
        transform.rotation = Quaternion.Euler(0,0, tonumber(z))
        if z >= 200 then
            for i, v in pairs(colliders:ToTable()) do
                v.enabled = true
            end
        end
        if z >= 300 then
            hitover = true
            hit = false
            UIs.barMask.fillAmount = 0.05
            transform.rotation = Quaternion.Euler(0,0, 180)
            for i, v in pairs(colliders:ToTable()) do
                v.enabled = false
            end
        end
    end
    
end



