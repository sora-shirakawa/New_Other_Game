# Wave Roguelike 実装プラン（第1弾）

## このコミットで追加した内容
- プレイ中のゲーム状態を管理する `GameManager`
- プレイヤー移動・基本ステータス・被弾処理
- 自動攻撃（最寄り敵を探索して弾を発射）
- 敵の追尾・接触ダメージ・死亡処理
- ダメージインターフェース `IDamageable`

## シーン構築メモ
1. `GameManager` を空オブジェクトにアタッチ
2. `Player` プレハブに以下をアタッチ
   - `PlayerController`
   - `PlayerStats`
   - `AutoAttackController`
   - Collider2D + Rigidbody2D(kinematic)
3. `Projectile` プレハブに
   - `Projectile`
   - Collider2D(isTrigger)
4. `Enemy` プレハブに
   - `EnemyController`
   - Collider2D + Rigidbody2D(dynamic)
5. 敵には `Enemy` タグを付与

## 次の実装予定
- Experience Orb とレベルアップ
- スキル3択UI
- WaveManager / SpawnManager
- Stage遷移とボス戦
