using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 注意把世界设置由center改为pivot
 * 等会设置敌人的finishAnimation
 * 等会在实现敌人攻击玩家时调用玩家方法就行
 * 注意把玩家和敌人的坐标中心都设置在脚上
 * 
 * 
 * 
 *接下来先完成卡牌效果接口的分割，具体是调用自己的
 *再完善角色参数 （一个有四个值的类型，实际上如果每个值又有一个血量属性，所以最好写一个Monobehaviour类，定义一下类所属的角色，类中定义义体类型，然后再定义一个这个类型的变量，然后再继承damageable类，重写死亡方法
 *这个类和player的整体damageable组件是不冲突的
 *但是我们要修改damageable组件，在，获取gamobject，以及(text方面,可不改
 *我们可以在初始化时,获取制作transform空物体，设为相应角色子物体,根据身体类型确定相对父物体位置，,添加并获取相关脚本类,（在character类里面完成，并让player继承start）,使用list，设置可见与否？
 *设置好layer和tag
 *
 *完善attack 和 attacked，
 *
 *在aroundEnemy中设置可以显示
 * */
