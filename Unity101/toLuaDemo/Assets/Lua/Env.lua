Env = {}
local this = Env
local ss = {}
local env

function this.InitEnv(obj0)
    env = obj0.gameObject
    print(gs)
    for i, v in pairs(gs) do
        ss[i] = v:GetComponent('SpriteRenderer').size
    end
end
function this.GenStage()
    GameObject.Instantiate(gs[1],Vector3(0,0,0),Quaternion.identity,env.transform)
    local rand = 1
    local count = 0
    local prev = 1
    for i = 1, mapLength do
        count = count + (ss[prev].x * 2) + (ss[rand].x *2)
        local rand1 = Random.Range(-2,4)
        GameObject.Instantiate(gs[rand],Vector3(count,rand1,0),Quaternion.identity,env.transform)
        if i == mapLength then
            GameObject.Instantiate(flag,Vector3(count + 1,rand1+1.5,0),Quaternion.identity,env.transform)
        end
        prev = rand
        rand = Random.Range(1,3)
        rand = math.ceil(rand)
    end
end



