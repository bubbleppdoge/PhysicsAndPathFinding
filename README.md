# PhysicsAndPathFinding

1.从资源包中任选一3d模型作为主角，另一模型作为敌人,其中敌人会动态走向主角（随着时间推移敌人会越走越快），碰到主角时游戏结束

2.可以操作主角进行位置移动，符合正常运动效果（有重力效果，不可翻越过高地形等）

3.场景中有相应的障碍物，主角与敌人均不可直接跨越障碍物

4.主角可对敌人进行攻击，攻击时主角播放攻击动作，击中敌人后敌人会后退一段距离且有被击中的地方有相应的粒子效果，击中一下敌人记一分。（粒子效果可直接使用untiy原生效果或者使用asset store中免费资源）

5.需要有练习2中相同的界面（开始游戏界面，最高分排行界面，结束界面）

6.本地保存游戏历史分数相关数据（保存前10高的分数即可）

ps:
1.改变了游戏内容，游戏要求碰到主角就结束，更改成玩家受到伤害生命归零结束。
2.主角立于地面的判断使用射线检测，因为不够熟练，所以有可能出现地面检测错误，角色出现例如：在半坡上移动到地面速度变快，突然可以攀越较高地势等的情况，暂无法自己解决，会问过师兄后再自行修改
3.第一次提交的排行榜界面因为测试不够，当时未发现同名无法保存多个名次的问题，现已更正
4.关于设置障碍物问题，因为场景中布置的树木可以充当障碍物，所以没有另外设置
5.游戏中保存的排行榜数据放置在Resources文件夹下，发布后的数据放在游戏放置文件夹在Resources文件夹中
6.游戏可通过方向键和wasd移动，鼠标左键攻击，鼠标改变视角。
