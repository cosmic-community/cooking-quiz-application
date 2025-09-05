import { Trophy, Medal, Award } from 'lucide-react'
import { getLeaderboard } from '@/lib/cosmic'

export default async function LeaderboardPage() {
  const results = await getLeaderboard(20)

  const getRankIcon = (rank: number) => {
    if (rank === 1) return <Trophy className="w-6 h-6 text-yellow-500" />
    if (rank === 2) return <Medal className="w-6 h-6 text-gray-400" />
    if (rank === 3) return <Award className="w-6 h-6 text-orange-600" />
    return <span className="w-6 h-6 flex items-center justify-center font-bold">{rank}</span>
  }

  const getRankStyle = (rank: number) => {
    if (rank === 1) return 'bg-gradient-to-r from-yellow-50 to-yellow-100 border-yellow-300'
    if (rank === 2) return 'bg-gradient-to-r from-gray-50 to-gray-100 border-gray-300'
    if (rank === 3) return 'bg-gradient-to-r from-orange-50 to-orange-100 border-orange-300'
    return 'bg-white'
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="max-w-4xl mx-auto">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold mb-4">Global Leaderboard</h1>
          <p className="text-gray-600">
            Top performers in our cooking quiz challenges
          </p>
        </div>

        {results.length > 0 ? (
          <div className="space-y-4">
            {results.map((result, index) => {
              const rank = index + 1
              const user = result.metadata.user
              const quiz = result.metadata.quiz
              
              return (
                <div
                  key={result.id}
                  className={`flex items-center p-4 rounded-lg border-2 ${getRankStyle(rank)} transition-transform hover:scale-105`}
                >
                  <div className="flex items-center justify-center w-12">
                    {getRankIcon(rank)}
                  </div>
                  
                  <div className="flex-grow ml-4">
                    <div className="flex items-center justify-between">
                      <div>
                        <h3 className="font-semibold text-lg">
                          {user?.metadata?.username || 'Anonymous'}
                        </h3>
                        <p className="text-gray-600 text-sm">
                          {quiz?.title || 'Unknown Quiz'}
                        </p>
                      </div>
                      
                      <div className="text-right">
                        <div className="text-2xl font-bold text-primary">
                          {result.metadata.score}%
                        </div>
                        <p className="text-gray-600 text-sm">
                          {result.metadata.correct_answers}/{result.metadata.total_questions} correct
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              )
            })}
          </div>
        ) : (
          <div className="text-center py-12 bg-white rounded-lg">
            <Trophy className="w-16 h-16 mx-auto mb-4 text-gray-400" />
            <p className="text-gray-600">
              No quiz results yet. Be the first to complete a quiz!
            </p>
          </div>
        )}
      </div>
    </div>
  )
}