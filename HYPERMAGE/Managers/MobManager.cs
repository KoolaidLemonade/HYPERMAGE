using HYPERMAGE.Models;
using HYPERMAGE.Particles;
using System.Collections.Generic;
using System.Linq;

namespace HYPERMAGE.Managers;

public static class MobManager
{
    public static readonly List<Mob> mobs = [];

    public static void SpawnMob(int id, Vector2 pos)
    {
        AddMob(new Mob(pos, id));
    }
    public static void AddMob(Mob mob)
    {
        mobs.Add(mob);
    }

    public static void UpdateMobs()
    {
        foreach (var mob in mobs.ToList())
        {
            mob.Update();
        }

        mobs.RemoveAll(mob => !mob.active);
    }
    public static void Update()
    {
        UpdateMobs();
    }

    public static void Draw()
    {
        foreach (var mob in mobs.ToList())
        {
            mob.Draw();
        }
    }

    public static void Clear()
    {
        mobs.Clear();
    }
}